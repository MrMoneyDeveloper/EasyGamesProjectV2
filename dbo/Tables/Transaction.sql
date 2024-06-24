CREATE TABLE [dbo].[Transaction]
(
    [TransactionID] BIGINT NOT NULL PRIMARY KEY,
    [Amount] DECIMAL(18, 2) NOT NULL,
    [TransactionTypeID] SMALLINT NOT NULL,
    [ClientID] INT NOT NULL,
    [Comment] NVARCHAR(100) NULL,
    FOREIGN KEY (TransactionTypeID) REFERENCES TransactionType(TransactionTypeID),
    FOREIGN KEY (ClientID) REFERENCES Client(ClientID)
);
