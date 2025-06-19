-- Script para crear la base de datos db_aba6a3_dianalc
-- SQL Server 2022

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'db_aba6a3_dianalc')
BEGIN
    CREATE DATABASE db_aba6a3_dianalc;
END
GO

-- Usar la base de datos
USE db_aba6a3_dianalc;
GO

-- Ejecutar el script de creación de tablas
-- (El contenido del archivo CreateDianaTables.sql debe ejecutarse después de esto)