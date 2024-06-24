CREATE PROCEDURE [dbo].[spUpdateClient]
    @ClientID INT,
    @Name NVARCHAR(50),
    @Surname NVARCHAR(50),
    @ClientBalance DECIMAL(18, 2)
AS
BEGIN
    UPDATE Client
    SET Name = @Name,
        Surname = @Surname,
        ClientBalance = @ClientBalance
    WHERE ClientID = @ClientID;
END
GO
