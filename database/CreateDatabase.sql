-- =============================================
-- Script de Creación de Base de Datos Tekus
-- Sistema de Gestión de Proveedores y Servicios
-- =============================================

USE master;
GO

-- Eliminar la base de datos si existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Tekus')
BEGIN
    ALTER DATABASE Tekus SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Tekus;
END
GO

-- Crear la base de datos
CREATE DATABASE Tekus;
GO

USE Tekus;
GO

-- =============================================
-- TABLAS
-- =============================================

-- Tabla: Countries
CREATE TABLE Countries (
    Code VARCHAR(2) NOT NULL,
    CodeAlpha3 VARCHAR(3) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Flag VARCHAR(500) NULL,
    CONSTRAINT PK_Countries PRIMARY KEY (Code)
);
GO

-- Tabla: Providers
CREATE TABLE Providers (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Nit VARCHAR(20) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NULL,
    UpdatedAt DATETIME2 NULL,
    UpdatedBy NVARCHAR(100) NULL,
    CONSTRAINT PK_Providers PRIMARY KEY (Id)
);
GO

-- Tabla: ProviderCustomFields
CREATE TABLE ProviderCustomFields (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ProviderId UNIQUEIDENTIFIER NOT NULL,
    FieldName NVARCHAR(100) NOT NULL,
    FieldValue NVARCHAR(1000) NOT NULL,
    FieldType NVARCHAR(50) NOT NULL DEFAULT 'text',
    Description NVARCHAR(500) NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NULL,
    UpdatedAt DATETIME2 NULL,
    UpdatedBy NVARCHAR(100) NULL,
    CONSTRAINT PK_ProviderCustomFields PRIMARY KEY (Id),
    CONSTRAINT FK_ProviderCustomFields_Providers FOREIGN KEY (ProviderId) 
        REFERENCES Providers(Id) ON DELETE CASCADE
);
GO

-- Tabla: Services
CREATE TABLE Services (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    HourlyRate DECIMAL(18, 2) NOT NULL,
    Description NVARCHAR(1000) NULL,
    ProviderId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NULL,
    UpdatedAt DATETIME2 NULL,
    UpdatedBy NVARCHAR(100) NULL,
    CONSTRAINT PK_Services PRIMARY KEY (Id),
    CONSTRAINT FK_Services_Providers FOREIGN KEY (ProviderId) 
        REFERENCES Providers(Id) ON DELETE CASCADE
);
GO

-- Tabla: ServiceCountries
CREATE TABLE ServiceCountries (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ServiceId UNIQUEIDENTIFIER NOT NULL,
    CountryCode VARCHAR(2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NULL,
    UpdatedAt DATETIME2 NULL,
    UpdatedBy NVARCHAR(100) NULL,
    CONSTRAINT PK_ServiceCountries PRIMARY KEY (Id),
    CONSTRAINT FK_ServiceCountries_Services FOREIGN KEY (ServiceId) 
        REFERENCES Services(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ServiceCountries_Countries FOREIGN KEY (CountryCode) 
        REFERENCES Countries(Code) ON DELETE NO ACTION
);
GO

-- =============================================
-- ÍNDICES
-- =============================================

-- Índices para Countries
CREATE NONCLUSTERED INDEX IX_Countries_CodeAlpha3 ON Countries(CodeAlpha3);
CREATE NONCLUSTERED INDEX IX_Countries_Name ON Countries(Name);
GO

-- Índices para Providers
CREATE UNIQUE NONCLUSTERED INDEX IX_Providers_Nit ON Providers(Nit);
CREATE NONCLUSTERED INDEX IX_Providers_Email ON Providers(Email);
CREATE NONCLUSTERED INDEX IX_Providers_Name ON Providers(Name);
GO

-- Índices para ProviderCustomFields
CREATE NONCLUSTERED INDEX IX_ProviderCustomFields_ProviderId ON ProviderCustomFields(ProviderId);
CREATE NONCLUSTERED INDEX IX_ProviderCustomFields_ProviderId_FieldName ON ProviderCustomFields(ProviderId, FieldName);
GO

-- Índices para Services
CREATE NONCLUSTERED INDEX IX_Services_ProviderId ON Services(ProviderId);
CREATE NONCLUSTERED INDEX IX_Services_Name ON Services(Name);
CREATE NONCLUSTERED INDEX IX_Services_HourlyRate ON Services(HourlyRate);
GO

-- Índices para ServiceCountries
CREATE NONCLUSTERED INDEX IX_ServiceCountries_ServiceId ON ServiceCountries(ServiceId);
CREATE NONCLUSTERED INDEX IX_ServiceCountries_CountryCode ON ServiceCountries(CountryCode);
CREATE UNIQUE NONCLUSTERED INDEX IX_ServiceCountries_ServiceId_CountryCode ON ServiceCountries(ServiceId, CountryCode);
GO

PRINT 'Base de datos Tekus creada exitosamente';
GO

