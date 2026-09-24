# Asisya DEV II — Frontend

SPA React + Vite. Implementa login JWT, localStorage, Axios interceptor, AuthGuard, listado/paginación/filtros, y formularios de creación/edición con `react-hook-form`.

## Ejecutar
1. Tener Node.js 20+.
2. Copiar `.env.example` a `.env` si se desea cambiar la URL.
3. Ejecutar:
```bash
npm install
npm run dev
```
4. Abrir la URL que indique Vite (normalmente `http://localhost:5173`).
5. Login demo: `admin` / `Admin123!`.

## Flujo
Login -> JWT en localStorage -> interceptor Axios agrega `Authorization: Bearer ...` -> AuthGuard protege `/products`.

## Backend
Por defecto apunta a `http://localhost:8080/api`. Si la API usa otra URL, cambiar `VITE_API_URL`.

## Nota
La prueba pide React JS y menciona "Reactive Forms"; en React no existe Angular Reactive Forms. Se implementó el equivalente idiomático mediante `react-hook-form`, con validaciones de formularios.
