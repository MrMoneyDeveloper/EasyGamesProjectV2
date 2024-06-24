USE UserDb;
GO

CREATE TABLE TransactionType (
    TransactionTypeID INT PRIMARY KEY,
    TransactionTypeName NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Client (
    ClientID INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incrementing ClientID
    Name NVARCHAR(50) NOT NULL,
    Surname NVARCHAR(50) NOT NULL,
    ClientBalance DECIMAL(18, 2) NOT NULL
);
GO

CREATE TABLE [Transaction] (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incrementing TransactionID
    Amount DECIMAL(18, 2) NOT NULL,
    TransactionTypeID INT FOREIGN KEY REFERENCES TransactionType(TransactionTypeID),
    ClientID INT FOREIGN KEY REFERENCES Client(ClientID),
    Comment NVARCHAR(200) NULL,
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE()  -- Add this line for TransactionDate
);
GO