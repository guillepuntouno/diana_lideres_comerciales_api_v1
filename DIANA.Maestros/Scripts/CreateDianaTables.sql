-- Script para crear las tablas de la base de datos db_aba6a3_dianalc
-- Compatible con SQL Server 2022
-- Servidor: SQL1004.site4now.net
-- Usuario: db_aba6a3_dianalc_admin

USE db_aba6a3_dianalc;
GO

-- Crear esquema si no existe
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'dbo')
BEGIN
    EXEC('CREATE SCHEMA dbo')
END
GO

-- Tabla: LideresComerciales
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LideresComerciales]') AND type in (N'U'))
CREATE TABLE [dbo].[LideresComerciales] (
    [Clave] NVARCHAR(20) NOT NULL PRIMARY KEY,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Pais] NVARCHAR(50) NOT NULL,
    [CentroDistribucion] NVARCHAR(100) NOT NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaActualizacion] DATETIME NULL,
    [Activo] BIT NOT NULL DEFAULT 1
);
GO

-- Tabla: Rutas
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rutas]') AND type in (N'U'))
CREATE TABLE [dbo].[Rutas] (
    [RutaId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] NVARCHAR(50) NOT NULL,
    [Asesor] NVARCHAR(100) NOT NULL,
    [LiderClave] NVARCHAR(20) NOT NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaActualizacion] DATETIME NULL,
    [Activo] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Rutas_LideresComerciales] FOREIGN KEY ([LiderClave]) 
        REFERENCES [dbo].[LideresComerciales]([Clave])
);
GO

-- Tabla: Negocios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Negocios]') AND type in (N'U'))
CREATE TABLE [dbo].[Negocios] (
    [Clave] NVARCHAR(20) NOT NULL PRIMARY KEY,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Canal] NVARCHAR(50) NOT NULL,
    [Clasificacion] NVARCHAR(10) NOT NULL,
    [Exhibidor] NVARCHAR(200) NULL,
    [RutaId] INT NOT NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaActualizacion] DATETIME NULL,
    [Activo] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Negocios_Rutas] FOREIGN KEY ([RutaId]) 
        REFERENCES [dbo].[Rutas]([RutaId])
);
GO

-- Tabla: PlanesTrabajos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanesTrabajos]') AND type in (N'U'))
CREATE TABLE [dbo].[PlanesTrabajos] (
    [PlanId] NVARCHAR(50) NOT NULL PRIMARY KEY,
    [LiderClave] NVARCHAR(20) NOT NULL,
    [Semana] INT NOT NULL,
    [Anio] INT NOT NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaActualizacion] DATETIME NULL,
    [Estatus] NVARCHAR(20) NULL DEFAULT 'borrador',
    CONSTRAINT [FK_PlanesTrabajos_LideresComerciales] FOREIGN KEY ([LiderClave]) 
        REFERENCES [dbo].[LideresComerciales]([Clave])
);
GO

-- Tabla: PlanDias
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanDias]') AND type in (N'U'))
CREATE TABLE [dbo].[PlanDias] (
    [PlanDiaId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PlanId] NVARCHAR(50) NOT NULL,
    [Dia] NVARCHAR(20) NOT NULL,
    [Objetivo] NVARCHAR(100) NOT NULL,
    [Tipo] NVARCHAR(50) NOT NULL,
    [CentroDistribucion] NVARCHAR(100) NULL,
    [RutaId] NVARCHAR(50) NULL,
    [RutaNombre] NVARCHAR(50) NULL,
    [TipoActividad] NVARCHAR(100) NULL,
    [Comentario] NVARCHAR(MAX) NULL,
    [Completado] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_PlanDias_PlanesTrabajos] FOREIGN KEY ([PlanId]) 
        REFERENCES [dbo].[PlanesTrabajos]([PlanId])
);
GO

-- Tabla: PlanDiaClientes
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlanDiaClientes]') AND type in (N'U'))
CREATE TABLE [dbo].[PlanDiaClientes] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PlanDiaId] INT NOT NULL,
    [ClienteId] NVARCHAR(20) NOT NULL,
    [ClienteNombre] NVARCHAR(100) NULL,
    [ClienteDireccion] NVARCHAR(200) NULL,
    [ClienteTipo] NVARCHAR(50) NULL,
    [Visitado] BIT NOT NULL DEFAULT 0,
    [FechaVisita] DATETIME NULL,
    CONSTRAINT [FK_PlanDiaClientes_PlanDias] FOREIGN KEY ([PlanDiaId]) 
        REFERENCES [dbo].[PlanDias]([PlanDiaId])
);
GO

-- Tabla: Visitas
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Visitas]') AND type in (N'U'))
CREATE TABLE [dbo].[Visitas] (
    [VisitaId] NVARCHAR(100) NOT NULL PRIMARY KEY,
    [LiderClave] NVARCHAR(20) NOT NULL,
    [ClienteId] NVARCHAR(20) NOT NULL,
    [ClienteNombre] NVARCHAR(100) NULL,
    [PlanId] NVARCHAR(50) NULL,
    [Dia] NVARCHAR(20) NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    
    -- Check-in fields
    [CheckInTimestamp] DATETIME NULL,
    [CheckInComentarios] NVARCHAR(MAX) NULL,
    [CheckInLatitud] DECIMAL(18,6) NULL,
    [CheckInLongitud] DECIMAL(18,6) NULL,
    [CheckInPrecision] DECIMAL(18,6) NULL,
    [CheckInDireccion] NVARCHAR(MAX) NULL,
    
    -- Check-out fields
    [CheckOutTimestamp] DATETIME NULL,
    [CheckOutComentarios] NVARCHAR(MAX) NULL,
    [CheckOutLatitud] DECIMAL(18,6) NULL,
    [CheckOutLongitud] DECIMAL(18,6) NULL,
    [CheckOutPrecision] DECIMAL(18,6) NULL,
    [CheckOutDireccion] NVARCHAR(MAX) NULL,
    [DuracionMinutos] INT NULL,
    
    [Estatus] NVARCHAR(20) NOT NULL DEFAULT 'en_proceso',
    [FechaModificacion] DATETIME NULL,
    [FechaFinalizacion] DATETIME NULL,
    [FechaCancelacion] DATETIME NULL,
    [MotivoCancelacion] NVARCHAR(MAX) NULL,
    [RutaId] INT NULL,
    
    CONSTRAINT [FK_Visitas_LideresComerciales] FOREIGN KEY ([LiderClave]) 
        REFERENCES [dbo].[LideresComerciales]([Clave]),
    CONSTRAINT [FK_Visitas_Negocios] FOREIGN KEY ([ClienteId]) 
        REFERENCES [dbo].[Negocios]([Clave]),
    CONSTRAINT [FK_Visitas_PlanesTrabajos] FOREIGN KEY ([PlanId]) 
        REFERENCES [dbo].[PlanesTrabajos]([PlanId]),
    CONSTRAINT [FK_Visitas_Rutas] FOREIGN KEY ([RutaId]) 
        REFERENCES [dbo].[Rutas]([RutaId])
);
GO

-- Tabla: VisitaFormularios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VisitaFormularios]') AND type in (N'U'))
CREATE TABLE [dbo].[VisitaFormularios] (
    [FormularioId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [VisitaId] NVARCHAR(100) NOT NULL UNIQUE,
    
    -- Sección 1 - Exhibidor
    [PoseeExhibidorAdecuado] BIT NOT NULL DEFAULT 0,
    [TipoExhibidorSeleccionado] NVARCHAR(MAX) NULL,
    [ModeloExhibidorSeleccionado] NVARCHAR(MAX) NULL,
    [CantidadExhibidores] INT NOT NULL DEFAULT 0,
    
    -- Sección 2 - Posicionamiento
    [PrimeraPosition] BIT NOT NULL DEFAULT 0,
    [Planograma] BIT NOT NULL DEFAULT 0,
    [PortafolioFoco] BIT NOT NULL DEFAULT 0,
    [Anclaje] BIT NOT NULL DEFAULT 0,
    
    -- Sección 3 - Categorías
    [Ristras] BIT NOT NULL DEFAULT 0,
    [Max] BIT NOT NULL DEFAULT 0,
    [Familiar] BIT NOT NULL DEFAULT 0,
    [Dulce] BIT NOT NULL DEFAULT 0,
    [Galleta] BIT NOT NULL DEFAULT 0,
    
    -- Sección 5 - Retroalimentación
    [Retroalimentacion] NVARCHAR(MAX) NULL,
    [Reconocimiento] NVARCHAR(MAX) NULL,
    
    [FechaActualizacion] DATETIME NULL,
    [Version] NVARCHAR(10) NULL,
    
    CONSTRAINT [FK_VisitaFormularios_Visitas] FOREIGN KEY ([VisitaId]) 
        REFERENCES [dbo].[Visitas]([VisitaId])
);
GO

-- Tabla: Compromisos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Compromisos]') AND type in (N'U'))
CREATE TABLE [dbo].[Compromisos] (
    [Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
    [Tipo] NVARCHAR(50) NOT NULL,
    [Detalle] NVARCHAR(200) NOT NULL,
    [Cantidad] INT NOT NULL DEFAULT 0,
    [Fecha] DATETIME NOT NULL,
    [ClienteId] NVARCHAR(20) NULL,
    [RutaId] NVARCHAR(100) NULL,
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
    [FormularioId] INT NOT NULL,
    CONSTRAINT [FK_Compromisos_VisitaFormularios] FOREIGN KEY ([FormularioId]) 
        REFERENCES [dbo].[VisitaFormularios]([FormularioId])
);
GO

-- Crear índices para mejorar el rendimiento
CREATE INDEX [IX_Rutas_LiderClave] ON [dbo].[Rutas]([LiderClave]);
CREATE INDEX [IX_Negocios_RutaId] ON [dbo].[Negocios]([RutaId]);
CREATE INDEX [IX_Visitas_LiderClave] ON [dbo].[Visitas]([LiderClave]);
CREATE INDEX [IX_Visitas_ClienteId] ON [dbo].[Visitas]([ClienteId]);
CREATE INDEX [IX_Visitas_PlanId] ON [dbo].[Visitas]([PlanId]);
CREATE INDEX [IX_Visitas_Estatus] ON [dbo].[Visitas]([Estatus]);
CREATE INDEX [IX_PlanesTrabajos_LiderClave] ON [dbo].[PlanesTrabajos]([LiderClave]);
CREATE INDEX [IX_PlanDias_PlanId] ON [dbo].[PlanDias]([PlanId]);
CREATE INDEX [IX_Compromisos_FormularioId] ON [dbo].[Compromisos]([FormularioId]);
GO