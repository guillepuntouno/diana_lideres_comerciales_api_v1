# Script para restaurar paquetes NuGet necesarios para Entity Framework
# Ejecutar desde la carpeta del proyecto DIANA.Maestros

Write-Host "Restaurando paquetes NuGet para DIANA.Maestros..." -ForegroundColor Green

# Restaurar todos los paquetes
dotnet restore

# O si prefieres usar NuGet Package Manager
# nuget restore ..\DIANA.Maestros.sln

# Instalar Entity Framework específicamente si es necesario
Install-Package EntityFramework -Version 6.4.4 -ProjectName DIANA.Maestros

Write-Host "Paquetes restaurados exitosamente." -ForegroundColor Green
Write-Host "Por favor, recompila el proyecto en Visual Studio." -ForegroundColor Yellow