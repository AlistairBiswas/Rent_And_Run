
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/14/2023 22:00:54
-- Generated from EDMX file: D:\Studies\Assignments and Offline\CSE3200\Let-Me-Drive\Letmedrive01\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [letdb01];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Bookings__CarID__182C9B23]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK__Bookings__CarID__182C9B23];
GO
IF OBJECT_ID(N'[dbo].[FK__Bookings__UserID__173876EA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK__Bookings__UserID__173876EA];
GO
IF OBJECT_ID(N'[dbo].[FK__Damage_de__CarID__1FCDBCEB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Damage_details] DROP CONSTRAINT [FK__Damage_de__CarID__1FCDBCEB];
GO
IF OBJECT_ID(N'[dbo].[FK__Reviews__CarID__37A5467C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT [FK__Reviews__CarID__37A5467C];
GO
IF OBJECT_ID(N'[dbo].[FK__Reviews__UserID__36B12243]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT [FK__Reviews__UserID__36B12243];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO
IF OBJECT_ID(N'[dbo].[Car_details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Car_details];
GO
IF OBJECT_ID(N'[dbo].[Damage_details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Damage_details];
GO
IF OBJECT_ID(N'[dbo].[Reviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reviews];
GO
IF OBJECT_ID(N'[dbo].[tbl_admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_admin];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [BookingID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [CarID] int  NOT NULL,
    [Start_date] nvarchar(50)  NULL,
    [Start_time] nvarchar(50)  NULL,
    [End_date] nvarchar(50)  NULL,
    [End_time] nvarchar(50)  NULL,
    [Start_place] nvarchar(max)  NOT NULL,
    [End_place] nvarchar(max)  NOT NULL,
    [TotalAmount] nvarchar(50)  NULL,
    [BookingStatus] nvarchar(50)  NULL,
    [TransectionID] varchar(50)  NULL,
    [BookingDate] varchar(255)  NULL
);
GO

-- Creating table 'Car_details'
CREATE TABLE [dbo].[Car_details] (
    [CarID] int IDENTITY(1000,1) NOT NULL,
    [CarNumber] nvarchar(50)  NOT NULL,
    [EngineNumber] nvarchar(50)  NOT NULL,
    [RegistrationYear] nvarchar(50)  NOT NULL,
    [CarDetails] nvarchar(max)  NULL,
    [CarImage] nvarchar(max)  NULL,
    [Daily_Fee] nvarchar(50)  NULL
);
GO

-- Adding a new column 'IsAvailable' to 'Car_details' table
ALTER TABLE [dbo].[Car_details]
ADD [IsAvailable] bit NOT NULL DEFAULT 1;

-- Creating table 'Damage_details'
CREATE TABLE [dbo].[Damage_details] (
    [DamageID] int IDENTITY(500,1) NOT NULL,
    [CarID] int  NOT NULL,
    [CarImage_Front] nvarchar(max)  NULL,
    [CarImage_Back] nvarchar(max)  NULL,
    [CarImage_Right] nvarchar(max)  NULL,
    [CarImage_Left] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_admin'
CREATE TABLE [dbo].[tbl_admin] (
    [ad_id] int IDENTITY(1000,1) NOT NULL,
    [ad_username] nvarchar(50)  NOT NULL,
    [ad_password] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [UserEmail] nvarchar(50)  NOT NULL,
    [UserPassword] nvarchar(50)  NOT NULL,
    [UserAddress] nvarchar(max)  NULL,
    [UserContact_number] nvarchar(50)  NULL,
    [UserPhoto] nvarchar(max)  NULL,
    [UserNID] nvarchar(max)  NULL,
    [UserDrivingID] nvarchar(max)  NULL,
    [UserStatus] nvarchar(50)  NULL,
    [UserDateOfBirth] nvarchar(50)  NULL
);
GO

-- Creating table 'Reviews'
CREATE TABLE [dbo].[Reviews] (
    [ReviewID] int IDENTITY(2000,1) NOT NULL,
    [UserID] int  NOT NULL,
    [CarID] int  NOT NULL,
    [Validation] nvarchar(50)  NULL,
    [Comment] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BookingID] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([BookingID] ASC);
GO

-- Creating primary key on [CarID] in table 'Car_details'
ALTER TABLE [dbo].[Car_details]
ADD CONSTRAINT [PK_Car_details]
    PRIMARY KEY CLUSTERED ([CarID] ASC);
GO

-- Creating primary key on [DamageID] in table 'Damage_details'
ALTER TABLE [dbo].[Damage_details]
ADD CONSTRAINT [PK_Damage_details]
    PRIMARY KEY CLUSTERED ([DamageID] ASC);
GO

-- Creating primary key on [ad_id] in table 'tbl_admin'
ALTER TABLE [dbo].[tbl_admin]
ADD CONSTRAINT [PK_tbl_admin]
    PRIMARY KEY CLUSTERED ([ad_id] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [ReviewID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [PK_Reviews]
    PRIMARY KEY CLUSTERED ([ReviewID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CarID] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK__Bookings__CarID__182C9B23]
    FOREIGN KEY ([CarID])
    REFERENCES [dbo].[Car_details]
        ([CarID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Bookings__CarID__182C9B23'
CREATE INDEX [IX_FK__Bookings__CarID__182C9B23]
ON [dbo].[Bookings]
    ([CarID]);
GO

-- Creating foreign key on [UserID] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK__Bookings__UserID__173876EA]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Bookings__UserID__173876EA'
CREATE INDEX [IX_FK__Bookings__UserID__173876EA]
ON [dbo].[Bookings]
    ([UserID]);
GO

-- Creating foreign key on [CarID] in table 'Damage_details'
ALTER TABLE [dbo].[Damage_details]
ADD CONSTRAINT [FK__Damage_de__CarID__1FCDBCEB]
    FOREIGN KEY ([CarID])
    REFERENCES [dbo].[Car_details]
        ([CarID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Damage_de__CarID__1FCDBCEB'
CREATE INDEX [IX_FK__Damage_de__CarID__1FCDBCEB]
ON [dbo].[Damage_details]
    ([CarID]);
GO

-- Creating foreign key on [CarID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK__Reviews__CarID__37A5467C]
    FOREIGN KEY ([CarID])
    REFERENCES [dbo].[Car_details]
        ([CarID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Reviews__CarID__37A5467C'
CREATE INDEX [IX_FK__Reviews__CarID__37A5467C]
ON [dbo].[Reviews]
    ([CarID]);
GO

-- Creating foreign key on [UserID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK__Reviews__UserID__36B12243]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Reviews__UserID__36B12243'
CREATE INDEX [IX_FK__Reviews__UserID__36B12243]
ON [dbo].[Reviews]
    ([UserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------