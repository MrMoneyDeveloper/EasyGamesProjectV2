CREATE TABLE [dbo].[Client]
(
    [ClientID] INT NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [Surname] NVARCHAR(50) NOT NULL,
    [ClientBalance] DECIMAL(18, 2) NOT NULL
);
