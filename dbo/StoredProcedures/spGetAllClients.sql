CREATE PROCEDURE [dbo].[spGetAllClients]
AS
BEGIN
    SELECT ClientID, Name, Surname, ClientBalance
    FROM Client;
END
GO
