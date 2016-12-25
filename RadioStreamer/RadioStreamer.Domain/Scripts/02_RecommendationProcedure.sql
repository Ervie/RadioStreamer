
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[CalculateRecommendation]

	@userName nvarchar(255)

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @UserId bigint;
	declare @ChannelsByDuration table(ChannelId bigint, ChannelName nvarchar(255), TotalDuration int);
	declare @TagsValues table(TagId bigint, TagName nvarchar(255), Value int);
	declare @ChannelValues table(ChannelId bigint, ChannelName nvarchar(255), Value int);

	Declare @Iterator int
	
	/* Get user Id */
	SELECT @UserId = Id
	FROM [dbo].[User]
	WHERE [Login] = @userName
	
	/* Get all channels ever listened by user */
	insert into @ChannelsByDuration (ChannelId, ChannelName, TotalDuration)
	SELECT chn.Id, chn.Name, SUM(his.Duration) as 'TotalDuration'
	FROM [dbo].History his, [dbo].Channel chn
	WHERE his.UserId = @UserId AND his.ChannelId = chn.Id
	GROUP BY chn.Id, chn.Name
	ORDER BY SUM(his.Duration) desc
	
	/* Load all tags into temporary table */
	
	INSERT INTO @TagsValues(TagId, TagName, Value)
	SELECT Id, Name, 0
	FROM [dbo].[Tag]
	
	/* Iterate through listened channels and fill  tags value according to duration */
	

	While (Select Count(*) From @ChannelsByDuration) > 0
	Begin

		Select Top 1 @Iterator = ChannelId From @ChannelsByDuration

		UPDATE tv
		SET tv.Value = tv.Value + cbd.TotalDuration
		FROM  @ChannelsByDuration cbd, TagsChannels tc, @TagsValues tv
		WHERE tv.TagId = tc.TagId AND @Iterator = tc.ChannelId


		Delete @ChannelsByDuration Where ChannelId = @Iterator

	End
	
	/* Load all channels into temporary table */

	INSERT INTO @ChannelValues(ChannelId, ChannelName, Value)
	SELECT Id, Name, 0
	FROM [dbo].[Channel]
	
	/* Iterate through listened channels and fill  tags value according to duration */
	
	While (Select Count(*) From @TagsValues) > 0
	Begin

		Select Top 1 @Iterator = TagId From @TagsValues

		UPDATE cv
		SET cv.Value = cv.Value + tv.Value
		FROM @ChannelValues cv, @TagsValues tv, TagsChannels tc
		WHERE @Iterator = tc.TagId AND cv.ChannelId = tc.ChannelId


		Delete @TagsValues Where TagId = @Iterator

	End
	
	SELECT * FROM @ChannelValues
    ORDER BY Value desc
    
END
GO
