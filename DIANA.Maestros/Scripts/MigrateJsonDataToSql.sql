-- Script para migrar datos desde JSON a SQL Server
-- Este script incluye datos de ejemplo basados en el JSON existente
-- Servidor: SQL1004.site4now.net
-- Base de datos: db_aba6a3_dianalc
-- Usuario: db_aba6a3_dianalc_admin

USE db_aba6a3_dianalc;
GO

-- Insertar Líderes Comerciales
INSERT INTO [dbo].[LideresComerciales] ([Clave], [Nombre], [Pais], [CentroDistribucion], [FechaCreacion], [Activo])
VALUES 
    ('LID001', 'LIDER CS - GRUPO A', 'El Salvador', 'Centro de Servicio', GETDATE(), 1),
    ('LID002', 'LIDER SA - GRUPO A', 'El Salvador', 'Distribuidora Santa Ana', GETDATE(), 1);
GO

-- Insertar Rutas
INSERT INTO [dbo].[Rutas] ([Nombre], [Asesor], [LiderClave], [FechaCreacion], [Activo])
VALUES 
    ('RUTACSD02', 'Doris Gómez', 'LID001', GETDATE(), 1),
    ('RUTASAD02', 'Doris Gómez', 'LID002', GETDATE(), 1);
GO

-- Obtener IDs de las rutas insertadas
DECLARE @RutaId1 INT = (SELECT RutaId FROM Rutas WHERE Nombre = 'RUTACSD02' AND LiderClave = 'LID001');
DECLARE @RutaId2 INT = (SELECT RutaId FROM Rutas WHERE Nombre = 'RUTASAD02' AND LiderClave = 'LID002');

-- Insertar Negocios para la primera ruta
INSERT INTO [dbo].[Negocios] ([Clave], [Nombre], [Canal], [Clasificacion], [Exhibidor], [RutaId], [FechaCreacion], [Activo])
VALUES 
    ('1002-1', 'Tienda La Esquinita', 'Canal D1', 'A', 'Exhibidor 01, Exhibido 02', @RutaId1, GETDATE(), 1),
    ('1002-2', 'Tienda El Ahorro', 'Canal D1', 'B', 'Exhibidor 01', @RutaId1, GETDATE(), 1),
    ('1002-3', 'Tienda Doña Marta', 'Canal D1', 'C', 'Exhibidor 02', @RutaId1, GETDATE(), 1),
    ('1002-4', 'Tienda El Buen Precio', 'Canal D1', 'A', 'No tiene', @RutaId1, GETDATE(), 1),
    ('1002-5', 'Tienda Mi Barrio', 'Canal D1', 'B', 'Exhibidor 01, Exhibido 02', @RutaId1, GETDATE(), 1),
    ('1002-6', 'Tienda El Amigo', 'Canal D1', 'C', 'Exhibidor 01', @RutaId1, GETDATE(), 1),
    ('1002-7', 'Tienda San José', 'Canal D1', 'A', 'Exhibidor 02', @RutaId1, GETDATE(), 1),
    ('1002-8', 'Tienda El Centro', 'Canal D1', 'B', 'No tiene', @RutaId1, GETDATE(), 1),
    ('1002-9', 'Tienda Los Primos', 'Canal D1', 'C', 'Exhibidor 01, Exhibido 02', @RutaId1, GETDATE(), 1),
    ('1002-10', 'Tienda El Roble', 'Canal D1', 'A', 'Exhibidor 01', @RutaId1, GETDATE(), 1),
    ('1002-11', 'Tienda La Esperanza', 'Canal D1', 'B', 'Exhibidor 02', @RutaId1, GETDATE(), 1),
    ('1002-12', 'Tienda Don Chepe', 'Canal D1', 'C', 'No tiene', @RutaId1, GETDATE(), 1),
    ('1002-13', 'Tienda El Portal', 'Canal D1', 'A', 'Exhibidor 01, Exhibido 02', @RutaId1, GETDATE(), 1),
    ('1002-14', 'Tienda La Bendición', 'Canal D1', 'B', 'Exhibidor 01', @RutaId1, GETDATE(), 1);

-- Insertar Negocios para la segunda ruta
INSERT INTO [dbo].[Negocios] ([Clave], [Nombre], [Canal], [Clasificacion], [Exhibidor], [RutaId], [FechaCreacion], [Activo])
VALUES 
    ('2002-1', 'Tienda La Esquinita', 'Canal D1', 'A', 'Exhibidor 01, Exhibido 02', @RutaId2, GETDATE(), 1),
    ('2002-2', 'Tienda El Ahorro', 'Canal D1', 'B', 'Exhibidor 01', @RutaId2, GETDATE(), 1),
    ('2002-3', 'Tienda Doña Marta', 'Canal D1', 'C', 'Exhibidor 02', @RutaId2, GETDATE(), 1),
    ('2002-4', 'Tienda El Buen Precio', 'Canal D1', 'A', 'No tiene', @RutaId2, GETDATE(), 1),
    ('2002-5', 'Tienda Mi Barrio', 'Canal D1', 'B', 'Exhibidor 01, Exhibido 02', @RutaId2, GETDATE(), 1),
    ('2002-6', 'Tienda El Amigo', 'Canal D1', 'C', 'Exhibidor 01', @RutaId2, GETDATE(), 1),
    ('2002-7', 'Tienda San José', 'Canal D1', 'A', 'Exhibidor 02', @RutaId2, GETDATE(), 1),
    ('2002-8', 'Tienda El Centro', 'Canal D1', 'B', 'No tiene', @RutaId2, GETDATE(), 1),
    ('2002-9', 'Tienda Los Primos', 'Canal D1', 'C', 'Exhibidor 01, Exhibido 02', @RutaId2, GETDATE(), 1),
    ('2002-10', 'Tienda El Roble', 'Canal D1', 'A', 'Exhibidor 01', @RutaId2, GETDATE(), 1),
    ('2002-11', 'Tienda La Esperanza', 'Canal D1', 'B', 'Exhibidor 02', @RutaId2, GETDATE(), 1),
    ('2002-12', 'Tienda Don Chepe', 'Canal D1', 'C', 'No tiene', @RutaId2, GETDATE(), 1),
    ('2002-13', 'Tienda El Portal', 'Canal D1', 'A', 'Exhibidor 01, Exhibido 02', @RutaId2, GETDATE(), 1),
    ('2002-14', 'Tienda La Bendición', 'Canal D1', 'B', 'Exhibidor 01', @RutaId2, GETDATE(), 1);

-- Insertar Plan de Trabajo de ejemplo
INSERT INTO [dbo].[PlanesTrabajos] ([PlanId], [LiderClave], [Semana], [Anio], [FechaCreacion], [Estatus])
VALUES ('LID001_SEM24', 'LID001', 24, 2025, '2025-06-13 05:02:28.873', 'borrador');

-- Insertar Visitas de ejemplo
INSERT INTO [dbo].[Visitas] 
    ([VisitaId], [LiderClave], [ClienteId], [ClienteNombre], [PlanId], [Dia], [FechaCreacion], 
     [CheckInTimestamp], [CheckInComentarios], [CheckInLatitud], [CheckInLongitud], [CheckInPrecision], [CheckInDireccion],
     [CheckOutTimestamp], [CheckOutComentarios], [CheckOutLatitud], [CheckOutLongitud], [CheckOutPrecision], [CheckOutDireccion],
     [DuracionMinutos], [Estatus], [FechaModificacion], [FechaFinalizacion], [RutaId])
VALUES 
    ('LID001_24_viernes_1002-1', 'LID001', '1002-1', 'Tienda La Esquinita', 'SIN_PLAN', 'viernes', 
     '2025-06-13 10:31:50.803', '2025-06-13 10:31:50.803', '', 0.0, 0.0, 0.0, '',
     '2025-06-13 10:32:26.869', '', 0.0, 0.0, 0.0, '', 0, 'completada', 
     '2025-06-13 10:32:25.151', '2025-06-13 10:32:26.869', @RutaId1),
    
    ('LID001_24_viernes_1002-2', 'LID001', '1002-2', 'Tienda El Ahorro', 'SIN_PLAN', 'viernes',
     '2025-06-13 11:45:00.416', '2025-06-13 11:45:00.416', '', 0.0, 0.0, 0.0, '',
     '2025-06-13 11:48:56.399', '', 0.0, 0.0, 0.0, '', 0, 'completada',
     '2025-06-13 11:48:52.014', '2025-06-13 11:48:56.399', @RutaId1);

-- Insertar Formularios de Visita
INSERT INTO [dbo].[VisitaFormularios]
    ([VisitaId], [PoseeExhibidorAdecuado], [CantidadExhibidores], [PrimeraPosition], [Planograma], 
     [PortafolioFoco], [Anclaje], [Ristras], [Max], [Familiar], [Dulce], [Galleta], 
     [Retroalimentacion], [Reconocimiento], [FechaActualizacion], [Version])
VALUES
    ('LID001_24_viernes_1002-1', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
     't1', 'tw', '2025-06-13 10:32:25.139', '1.0'),
    ('LID001_24_viernes_1002-2', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
     'test', 'test 2', '2025-06-13 11:48:52.002', '1.0');

-- Mensaje de finalización
PRINT 'Migración de datos completada exitosamente.';
GO