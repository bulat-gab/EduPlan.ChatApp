USE chatapp

SET IDENTITY_INSERT [dbo].[Message] ON;
GO

-- INSERT INTO [Message] (Id, ChatId, FromId, ToId, Text, CreatedAt, CreatedBy)
-- VALUES (5, 1, 2, 1, 'What about you?', GETDATE(), 2)
-- GO

-- User1
INSERT INTO [Message] (Id, ChatId, FromId, ToId, Text, CreatedAt, CreatedBy)
VALUES (6, 1, 1, 2, 'Good, thanks. Are you free tonight?', GETDATE(), 1)
GO

-- User2
INSERT INTO [Message] (Id, ChatId, FromId, ToId, Text, CreatedAt, CreatedBy)
VALUES (7, 1, 2, 1, 'Im playing a football. Wanna join?', GETDATE(), 2)
GO

-- User1
INSERT INTO [Message] (Id, ChatId, FromId, ToId, Text, CreatedAt, CreatedBy)
VALUES (8, 1, 1, 2, 'Sure, what time?', GETDATE(), 1)
GO