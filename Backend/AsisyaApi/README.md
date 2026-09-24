# Asisya DEV II — Backend

## 1. Objetivo
Implementación de la prueba técnica: API REST .NET 8, arquitectura por capas, DTOs con mapeo explícito, PostgreSQL, carga masiva asíncrona, JWT, Swagger, Docker, CI y pruebas. La especificación solicita CRUD, consulta avanzada, 100.000 productos, categorías SERVIDORES/CLOUD, performance, seguridad, pruebas y despliegue en Docker/CI. 

## 2. Arquitectura
- `Domain`: entidades puras (`Product`, `Category`).
- `Application`: DTOs, contratos y servicios; no conoce EF Core.
- `Infrastructure`: PostgreSQL/EF Core, repositorios y background queue.
- `Api`: HTTP, autenticación JWT y Swagger.
- `Tests`: pruebas unitarias.

Flujo: Controller -> Application Service -> Repository interface -> Infrastructure repository -> EF Core -> PostgreSQL.

### DTOs y mapeo
Las entidades nunca son tipos de entrada/salida de los controllers. `EntityMappings.cs` realiza el mapeo explícito `DTO <-> Entity`.

## 3. Requisitos
- .NET SDK 8
- Docker Desktop
- PostgreSQL solo si se ejecuta sin Docker.

## 4. Ejecutar con Docker
```bash
docker compose up --build
```
API: `http://localhost:8080/swagger`

La base queda en PostgreSQL `localhost:5432`, usuario `postgres`, contraseña `postgres`, BD `asisya`.

## 5. Ejecutar localmente
```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/AsisyaApi.Api
```
Para ejecución local asegúrate de tener PostgreSQL y usa la cadena de `appsettings.json`.

## 6. Swagger — pruebas paso a paso
### A. Login
`POST /api/Auth/login`
```json
{"username":"admin","password":"Admin123!"}
```
Copia `token`. En Swagger pulsa **Authorize** y escribe:
`Bearer <token>`

### B. Crear categorías
`POST /api/Category`
```json
{"categoryName":"SERVIDORES","description":"Infraestructura de servidores","picture":"https://picsum.photos/seed/servers/300/200"}
```
Repite con:
```json
{"categoryName":"CLOUD","description":"Servicios cloud","picture":"https://picsum.photos/seed/cloud/300/200"}
```
Guarda los IDs.

### C. Crear un producto
`POST /api/Product`
```json
{
  "productName":"Servidor Dell PowerEdge",
  "categoryId":1,
  "quantityPerUnit":"1 unidad",
  "unitPrice":4500.00,
  "unitsInStock":10,
  "unitsOnOrder":2,
  "reorderLevel":3,
  "discontinued":false
}
```

### D. Consulta avanzada
`GET /api/Product?pageNumber=1&pageSize=20&search=cloud&minPrice=10&maxPrice=5000`
También se puede filtrar por `categoryId` y `discontinued`.

### E. Detalle
`GET /api/Product/{id}` devuelve el producto y `categoryPicture`.

### F. Actualizar
`PUT /api/Product/{id}`
```json
{
  "productName":"Servidor actualizado",
  "categoryId":1,
  "quantityPerUnit":"2 unidades",
  "unitPrice":4700,
  "unitsInStock":8,
  "unitsOnOrder":1,
  "reorderLevel":2,
  "discontinued":false
}
```

### G. Borrar
`DELETE /api/Product/{id}`.

### H. Carga de 100.000
`POST /api/Product/bulk`
```json
{"quantity":100000,"categoryIds":[1,2],"batchSize":2000}
```
La API responde `202 Accepted` con `jobId`. Consultar:
`GET /api/Product/bulk/{jobId}`
hasta que `status` sea `Completed`.

## 7. Performance
La carga masiva usa una cola en memoria, un scope nuevo por trabajo y lotes de 2.000 entidades. Esto evita mantener el request HTTP abierto y limita el tamaño del ChangeTracker por lote.

Para producción multi-instancia, sustituir la cola/store en memoria por un broker durable (por ejemplo RabbitMQ/Azure Service Bus/SQS) y un store distribuido (Redis/DB). Para horizontal scaling, ejecutar múltiples réplicas detrás de un load balancer y mantener JWT stateless y PostgreSQL como persistencia compartida.

## 8. Seguridad
JWT protege los endpoints de categorías y productos. El login demo está pensado para la prueba. En producción, usuarios/contraseñas deben residir en un Identity Provider o base de datos con hash seguro y el secreto JWT debe venir de un secret manager.

## 9. Docker y CI
`Dockerfile` construye/publica la API. `docker-compose.yml` levanta PostgreSQL + API. `.github/workflows/ci.yml` ejecuta restore, build, test y docker build.

## 10. Decisiones y límites
- PostgreSQL fue elegido porque la prueba lo recomienda.
- Mapeo manual para mantener claro el límite entre DTO y dominio.
- La cola in-memory es suficiente para demostrar asincronía en una instancia; no es un mecanismo durable de producción.
- Las migraciones de EF pueden añadirse con `dotnet ef migrations add InitialCreate` y `dotnet ef database update`; el compose de la prueba usa el modelo de EF al ejecutar el esquema según la estrategia elegida. Para un pipeline productivo, versionar las migraciones.
