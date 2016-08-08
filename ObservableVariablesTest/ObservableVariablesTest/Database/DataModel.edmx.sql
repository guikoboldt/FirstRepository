
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/07/2016 23:18:43
-- Generated from EDMX file: C:\Users\Guilherme\SkyDrive\C#\GitHub Projects\FirstRepository\ObservableVariablesTest\ObservableVariablesTest\Database\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TEST_OPP];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[VariablesValuesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VariablesValuesSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'VariablesValuesSet'
CREATE TABLE [dbo].[VariablesValuesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [idData] int  NOT NULL,
    [ValueSender] nvarchar(100)  NOT NULL,
    [ValueArgs] nvarchar(100)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'VariablesValuesSet'
ALTER TABLE [dbo].[VariablesValuesSet]
ADD CONSTRAINT [PK_VariablesValuesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------