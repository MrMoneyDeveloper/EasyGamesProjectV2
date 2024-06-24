CREATE PROCEDURE [dbo].[spGetClientByID]
    @ClientID INT
AS
BEGIN
    SELECT ClientID, Name, Surname, ClientBalance
    FROM Client
    WHERE ClientID = @ClientID;
END
GO
