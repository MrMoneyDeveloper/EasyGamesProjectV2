-- Drop the specific stored procedure if it exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAddTransaction]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[spAddTransaction];
END
GO

-- Drop the database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'UserDb')
BEGIN
    ALTER DATABASE [UserDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [UserDb];
END
GO

-- Create the database
CREATE DATABASE [UserDb];
GO
