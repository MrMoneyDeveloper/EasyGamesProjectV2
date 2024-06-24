DROP PROCEDURE IF EXISTS [dbo].[spAddTransaction]
GO

CREATE PROCEDURE [dbo].[spAddTransaction]
    @ClientID INT,
    @Amount DECIMAL(18, 2),
    @TransactionTypeID INT,
    @Comment NVARCHAR(200) OUTPUT
AS
BEGIN
    DECLARE @ClientBalance DECIMAL(18, 2);

    -- Check if the transaction is valid and update ClientBalance accordingly
    IF @TransactionTypeID = 2 AND (SELECT ClientBalance FROM Client WHERE ClientID = @ClientID) >= @Amount -- Losing (Credit)
    BEGIN
        UPDATE Client
        SET ClientBalance = ClientBalance - @Amount
        WHERE ClientID = @ClientID;

        SET @Comment = 'Losing';

        INSERT INTO [Transaction] (Amount, TransactionTypeID, ClientID, Comment, TransactionDate)
        VALUES (-@Amount, @TransactionTypeID, @ClientID, @Comment, GETDATE());
    END
    ELSE IF @TransactionTypeID = 1 -- Winnings (Debit)
    BEGIN
        UPDATE Client
        SET ClientBalance = ClientBalance + @Amount
        WHERE ClientID = @ClientID;

        SET @Comment = 'Winnings';

        INSERT INTO [Transaction] (Amount, TransactionTypeID, ClientID, Comment, TransactionDate)
        VALUES (@Amount, @TransactionTypeID, @ClientID, @Comment, GETDATE());
    END

    -- Return the updated balance
    SELECT ClientBalance AS NewBalance
    FROM Client
    WHERE ClientID = @ClientID;
END;
