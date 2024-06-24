CREATE PROCEDURE [dbo].[spAddClient]
    @Name NVARCHAR(50),
    @Surname NVARCHAR(50),
    @ClientBalance DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO Client (Name, Surname, ClientBalance)
    VALUES (@Name, @Surname, @ClientBalance);
END
GO
