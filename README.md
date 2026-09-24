# Solución API REST

## Arquitectura:
Se crea proyecto en una arquitectura de capas, esta estructura nos permite tener la 
presentación, lógica de negocio, acceso a datos y contratos separados logrando una 
mantenibilidad  y escalabilidad efectiva evitando que el código se convierta en un 
monolito difícil de mantener y por último nos permite aplicar políticas de seguridad(Auth-JWT).

presentación: Frontend en React 
API: Gestión de las peticiones HTTP
Application: Lógica de la aplicación (validaciones ,DTOs, interfaces)
Infraestructure: Modelos y persistencia en la base de datos 
Domain: Entidades
Test: Pruebas Unitarias

## Tecnologías/requisitos

- C#
- .NET 8
- Entity Framework Core
- PostgreSQL
- Docker
- react
- node.js v20-18-0
- npm v10.8.2

## Instalación y ejecución 

BACKEND
1. Clonar repositorio:
git clone https://github.com/nescobar11/TestDevII.git
cd Backend 
dotnet restore
dotnet build
dotnet test


2. base de datos 
cd backend
docker compose up -d
docker compose ps --> valida que postgres este Arriba
en el navegador: http://localhost:8081/
user: postgres
pass: postgres

3. construir tablas
Add-Migration InitialCreate

3. Ejecutar backend/Swagger
dotnet run --project AsisyaApi.Api
en el navegador:https://localhost:55310/swagger
credenciales para pruebas:
{"username":"admin","password":"Admin123!"}

FRONTEND
npm install
npm run dev
```



