# Database Integration Documentation

## Connection String Configuration

Add the following connection string to your `Web.config` file:

```xml
<connectionStrings>
  <add name="DianaConnection" 
       connectionString="Data Source=YOUR_SERVER;Initial Catalog=DIANA_DB;Integrated Security=True;MultipleActiveResultSets=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Entity Framework Configuration

Add Entity Framework to your project via NuGet:

```powershell
Install-Package EntityFramework -Version 6.4.4
```

## Migration Commands

To create and update the database using Entity Framework migrations:

```powershell
# Enable migrations
Enable-Migrations -ProjectName DIANA.Maestros

# Add initial migration
Add-Migration InitialCreate -ProjectName DIANA.Maestros

# Update database
Update-Database -ProjectName DIANA.Maestros
```

## Dependency Injection Setup

For using repositories in your controllers, consider using a DI container like Unity:

```csharp
// In WebApiConfig.cs
public static void Register(HttpConfiguration config)
{
    var container = new UnityContainer();
    
    // Register your dependencies
    container.RegisterType<DianaDbContext>(new HierarchicalLifetimeManager());
    container.RegisterType<IVisitaRepository, VisitaRepository>();
    
    config.DependencyResolver = new UnityDependencyResolver(container);
    
    // Rest of your configuration...
}
```

## Sample Refactored Controller

Here's how the VisitasController would look with Entity Framework:

```csharp
public class VisitasController : ApiController
{
    private readonly IVisitaRepository _visitaRepository;
    
    public VisitasController(IVisitaRepository visitaRepository)
    {
        _visitaRepository = visitaRepository;
    }
    
    [HttpPost]
    [Route("api/visitas")]
    public async Task<IHttpActionResult> CrearVisita([FromBody] VisitaCreateDto dto)
    {
        // Map DTO to Entity
        var visita = new VisitaEntity
        {
            VisitaId = dto.ClaveVisita,
            LiderClave = dto.LiderClave,
            ClienteId = dto.ClienteId,
            // ... map other properties
        };
        
        var result = await _visitaRepository.CrearVisitaAsync(visita);
        
        // Map Entity to Response DTO
        return Ok(MapToResponseDto(result));
    }
}
```

## Data Migration Script

To migrate existing JSON data to SQL:

```csharp
public class DataMigrationService
{
    public void MigrateJsonToSql()
    {
        using (var context = new DianaDbContext())
        {
            // Read JSON files
            var jsonPath = HostingEnvironment.MapPath("~/App_Data/lideres_comerciales_completo.json");
            var json = File.ReadAllText(jsonPath);
            var lideres = JsonConvert.DeserializeObject<List<LiderComercial>>(json);
            
            // Convert and save to database
            foreach (var lider in lideres)
            {
                var entity = new LiderComercialEntity
                {
                    Clave = lider.Clave,
                    Nombre = lider.Nombre,
                    // ... map other properties
                };
                
                context.LideresComerciales.Add(entity);
            }
            
            context.SaveChanges();
        }
    }
}
```