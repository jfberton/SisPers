
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/05/2015 17:57:52
-- Generated from EDMX file: C:\Users\Berton\Desktop\SISPERSONAL\SisPerBootstrap\SisPer\Aplicativo\HuellasModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ClockCard];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FICHADA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FICHADA];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FICHADA'
CREATE TABLE [dbo].[FICHADA] (
    [FIC_ID] int IDENTITY(1,1) NOT NULL,
    [LEG_LEGAJO] int  NULL,
    [FIC_TARJETA] int  NOT NULL,
    [FIC_FECHA] datetime  NOT NULL,
    [FIC_HORA] char(5)  NOT NULL,
    [FIC_ENTSAL] char(1)  NOT NULL,
    [FIC_RELOJ] int  NULL,
    [FIC_ORIGEN] char(1)  NOT NULL,
    [FIC_NOVEDAD] int  NULL,
    [FIC_EQUIPO] char(10)  NULL,
    [FIC_NOTAS] varchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [FIC_ID] in table 'FICHADA'
ALTER TABLE [dbo].[FICHADA]
ADD CONSTRAINT [PK_FICHADA]
    PRIMARY KEY CLUSTERED ([FIC_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------