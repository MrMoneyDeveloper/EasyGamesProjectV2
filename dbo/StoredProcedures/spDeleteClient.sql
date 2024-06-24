CREATE PROCEDURE spDeleteClient
    @ClientID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM [Transaction] WHERE ClientID = @ClientID)
    BEGIN
        RAISERROR ('Cannot delete client because there are related transactions.', 16, 1);
        RETURN;
    END

    DELETE FROM Client WHERE ClientID = @ClientID;
END
