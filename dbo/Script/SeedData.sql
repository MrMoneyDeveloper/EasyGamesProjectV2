USE UserDb;
GO

-- Insert data into TransactionType
INSERT INTO TransactionType (TransactionTypeID, TransactionTypeName) VALUES (1, 'Debit'), (2, 'Credit');
GO

-- Insert data into Client
INSERT INTO Client (Name, Surname, ClientBalance)
VALUES 
('Peter', 'Parker', 0.00),
('Tony', 'Stark', 0.00),
('Bruce', 'Banner', 0.00),
('Clark', 'Kent', 0.00),
('Diana', 'Prince', 0.00),
('Barry', 'Allen', 0.00),
('Hal', 'Jordan', 0.00),
('Arthur', 'Curry', 0.00),
('Victor', 'Stone', 0.00),
('Steve', 'Rogers', 0.00),
('Natasha', 'Romanoff', 0.00),
('Clint', 'Barton', 0.00),
('Wanda', 'Maximoff', 0.00),
('Stephen', 'Strange', 0.00),
('TChalla', 'Black Panther', 0.00),
('Scott', 'Lang', 0.00),
('Hope', 'van Dyne', 0.00),
('Sam', 'Wilson', 0.00),
('Bucky', 'Barnes', 0.00),
('Carol', 'Danvers', 0.00),
('Peter', 'Quill', 0.00),
('Gamora', 'Thanos', 0.00),
('Drax', 'Destroyer', 0.00),
('Rocket', 'Raccoon', 0.00),
('Groot', 'Tree', 0.00),
('Bruce', 'Wayne', 0.00),
('Selina', 'Kyle', 0.00),
('Harley', 'Quinn', 0.00),
('Poison', 'Ivy', 0.00),
('Dick', 'Grayson', 0.00),
('Jason', 'Todd', 0.00),
('Tim', 'Drake', 0.00),
('Damian', 'Wayne', 0.00),
('Barbara', 'Gordon', 0.00),
('Kate', 'Kane', 0.00),
('Kara', 'Zor-El', 0.00),
('Oliver', 'Queen', 0.00),
('Dinah', 'Lance', 0.00),
('Ray', 'Palmer', 0.00),
('Sara', 'Lance', 0.00),
('John', 'Constantine', 0.00),
('Zatanna', 'Zatara', 0.00),
('Raven', 'Azarath', 0.00),
('Beast', 'Boy', 0.00),
('Cyborg', 'Teen Titan', 0.00),
('Starfire', 'Princess', 0.00),
('Wonder', 'Girl', 0.00),
('Kid', 'Flash', 0.00),
('Arsenal', 'Roy Harper', 0.00);
GO

-- Insert transactions ensuring balance does not become negative
DECLARE @Counter INT = 1;
DECLARE @TransactionTypeID INT;
DECLARE @ClientID INT;
DECLARE @Amount DECIMAL(18, 2);
DECLARE @ClientBalance DECIMAL(18, 2);

WHILE @Counter <= 300
BEGIN
    SELECT TOP 1 @ClientID = ClientID, @ClientBalance = ClientBalance FROM Client ORDER BY NEWID();
    SET @TransactionTypeID = CASE WHEN RAND() > 0.5 THEN 1 ELSE 2 END;
    SET @Amount = ROUND(RAND() * 10000, 2);

    -- Check if the transaction is valid and update ClientBalance accordingly
    IF @TransactionTypeID = 2 AND @ClientBalance >= @Amount -- Losing (Credit)
    BEGIN
        UPDATE Client
        SET ClientBalance = ClientBalance - @Amount
        WHERE ClientID = @ClientID;

        INSERT INTO [Transaction] (Amount, TransactionTypeID, ClientID, Comment, TransactionDate)
        VALUES (-@Amount, @TransactionTypeID, @ClientID, 'Losing', DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()));
    END
    ELSE IF @TransactionTypeID = 1 -- Winnings (Debit)
    BEGIN
        UPDATE Client
        SET ClientBalance = ClientBalance + @Amount
        WHERE ClientID = @ClientID;

        INSERT INTO [Transaction] (Amount, TransactionTypeID, ClientID, Comment, TransactionDate)
        VALUES (@Amount, @TransactionTypeID, @ClientID, 'Winnings', DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()));
    END

    SET @Counter = @Counter + 1;
END;
GO
