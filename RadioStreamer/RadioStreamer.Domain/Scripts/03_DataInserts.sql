use [RadioStreamerDB]

SET IDENTITY_INSERT dbo.Channel ON

INSERT INTO [dbo].[Channel] (Id, Name, PageUrl, StreamUrl) VALUES
    (34,'Touhou Radio','https://www.touhouradio.com/','http://37.59.41.178:8000/;'),
    (35, 'RMF Game Music', 'http://www.rmfon.pl/play,156', 'http://185.69.192.87/GAMEMUSIC');


SET IDENTITY_INSERT dbo.Channel OFF
    
----------------------------------------------

SET IDENTITY_INSERT dbo.TagsChannels ON

INSERT INTO [TagsChannels] (id, ChannelId, TagId) VALUES
    (149,34,14),
	(150,34,23),
	(151,34,24),
    (152,35,13),
    (153,35,23);

SET IDENTITY_INSERT dbo.TagsChannels OFF