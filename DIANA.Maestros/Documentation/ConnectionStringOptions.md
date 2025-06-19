# Opciones de Cadenas de Conexión para Base de Datos

## Información de la Base de Datos
- **Nombre de la BD**: db_aba6a3_dianalc
- **Usuario**: db_aba6a3_dianalc_admin
- **Contraseña**: Diana2025

## Opciones de Cadena de Conexión

### Opción 1: Servidor Remoto Somee.com (Recomendada)
```xml
<add name="DianaConnection" 
     connectionString="Server=tcp:db_aba6a3_dianalc.mssql.somee.com,1433;Initial Catalog=db_aba6a3_dianalc;User ID=db_aba6a3_dianalc_admin;Password=Diana2025;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True" 
     providerName="System.Data.SqlClient" />
```

### Opción 2: Servidor Remoto sin Puerto Específico
```xml
<add name="DianaConnection" 
     connectionString="Data Source=db_aba6a3_dianalc.mssql.somee.com;Initial Catalog=db_aba6a3_dianalc;User ID=db_aba6a3_dianalc_admin;Password=Diana2025;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True" 
     providerName="System.Data.SqlClient" />
```

### Opción 3: Con Integrated Security Deshabilitada
```xml
<add name="DianaConnection" 
     connectionString="Server=db_aba6a3_dianalc.mssql.somee.com;Database=db_aba6a3_dianalc;User Id=db_aba6a3_dianalc_admin;Password=Diana2025;Trusted_Connection=False;MultipleActiveResultSets=True" 
     providerName="System.Data.SqlClient" />
```

### Opción 4: Para SQL Server Express Local (si aplica)
```xml
<add name="DianaConnection" 
     connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=db_aba6a3_dianalc;User ID=db_aba6a3_dianalc_admin;Password=Diana2025;MultipleActiveResultSets=True" 
     providerName="System.Data.SqlClient" />
```

## Pasos para Troubleshooting

1. **Verificar que el servidor está activo**
   - Puedes usar herramientas como SQL Server Management Studio
   - O usar `telnet db_aba6a3_dianalc.mssql.somee.com 1433`

2. **Verificar credenciales**
   - Asegúrate de que el usuario y contraseña sean correctos

3. **Verificar firewall**
   - Puede ser que el firewall esté bloqueando la conexión

4. **Probar con diferentes opciones de encriptación**
   - Algunas configuraciones requieren `Encrypt=False`

## Configuración Actual en Uso
La aplicación está usando la **Opción 1** por defecto.

Si ninguna funciona, es posible que necesites contactar al proveedor de hosting para obtener la cadena de conexión correcta.