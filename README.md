# FinServe Complete Template

This zip contains a runnable template:

- Backend: src/FinServe.Api (dotnet 9) with Auth DB context, JWT generator, minimal Auth endpoints.
- Frontend: frontend/webapp (React + Vite) combined Admin + Customer UI with protected routing.

Run backend:
- `cd src/FinServe.Api` then `dotnet restore` and `dotnet run`

Run frontend:
- `cd frontend/webapp` then `npm install` and `npm run dev`

Notes: update MySQL connection in appsettings.json and change Jwt:Secret for production.
