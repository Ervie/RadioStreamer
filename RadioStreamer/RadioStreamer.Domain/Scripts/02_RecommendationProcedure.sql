
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CalculateRecommendation]

	@userName nvarchar(255)

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @UserId bigint;
	declare @ChannelsByDuration table(ChannelId bigint, ChannelName nvarchar(255), TotalDuration int);
	declare @TagsValues table(TagId bigint, TagName nvarchar(255), Value int);
	declare @ChannelValues table(ChannelId bigint, ChannelName nvarchar(255), StreamUrl nvarchar(255), Value float);
	declare @AverageRatings table (ChannelId bigint, ChannelName nvarchar(255), AverageRating float);

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
	ORDER BY SUM(his.Value) desc
	
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

	INSERT INTO @ChannelValues(ChannelId, ChannelName, StreamUrl, Value)
	SELECT Id, Name, StreamUrl,  0.0
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
	
	/* Calculate Average Rating */
	
	INSERT INTO @AverageRatings (ChannelId, ChannelName, AverageRating)
	SELECT chn.Id, chn.Name, AVG(Cast(r.Value as float))
	FROM Channel chn, Rating r
	WHERE chn.Id = r.ChannelId
	GROUP BY chn.Id, chn.Name
	
	/* Set Avg Rating to 5/10 for those without ratings */
	
	INSERT INTO @AverageRatings (ChannelId, ChannelName, AverageRating)
	SELECT chn.Id, chn.Name, 5.0
	FROM Channel chn
	WHERE chn.Id NOT IN (SELECT ChannelId from @AverageRatings)
	
	/* Multiply Channel Values by their avg rating */
	
	UPDATE cv
	SET cv.Value = cv.Value * ar.AverageRating / 10.0
	FROM @ChannelValues cv, @AverageRatings ar
	WHERE cv.ChannelId = ar.ChannelId
	
	/* Selects top 3 by value and not in favourites */
	
	SELECT TOP 3 cv.ChannelId, cv.ChannelName, cv.StreamUrl, cv.Value
	FROM @ChannelValues cv
	WHERE cv.ChannelId NOT IN (SELECT f.ChannelId FROM Favourite f WHERE f.UserId = @UserId)
    ORDER BY Value desc
    
END
GO
