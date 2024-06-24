CREATE PROCEDURE [dbo].[spGetTransactionsByClientId]
    @ClientID INT
AS
BEGIN
    SELECT 
        t.TransactionID,
        t.Amount,
        tt.TransactionTypeName,
        t.Comment,
        t.TransactionDate
    FROM [Transaction] t
    INNER JOIN TransactionType tt ON t.TransactionTypeID = tt.TransactionTypeID
    WHERE t.ClientID = @ClientID
    ORDER BY t.TransactionDate DESC;
END
GO
