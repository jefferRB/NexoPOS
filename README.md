# NexoPOS

Sistema de punto de venta e inventario multi-sucursal. Este repositorio contiene
la maqueta funcional para presentación al cliente, adaptada a la operación de
**Grupo Veterinario Demo** (tres veterinarias).

> ⚠️ **Datos de demostración.** Esta iteración muestra una funcionalidad vertical
> real (React → Web API → servicios de aplicación → repositorio en memoria).
> **No** hay base de datos, autenticación real ni integración fiscal real todavía.
> Los indicadores, sucursales, servicios, botiquines, comprobantes y movimientos
> son datos de ejemplo y están rotulados como tales en la interfaz.

## Stack

- **.NET 10** · ASP.NET Core Web API · Arquitectura limpia
- **React 19 + TypeScript** (Vite) · gestor de paquetes **npm** · router propio mínimo
- Pruebas: **xUnit** (unitarias e integración)

## Estructura

```
NexoPOS
├── src
│   ├── NexoPOS.Domain          # Entidades y reglas de negocio (sin dependencias)
│   ├── NexoPOS.Application      # DTOs, interfaces y servicios de aplicación
│   ├── NexoPOS.Infrastructure   # Repositorio y datos demo en memoria
│   └── NexoPOS.API              # Web API (controladores, CORS, DI)
├── tests
│   ├── NexoPOS.UnitTests        # Lógica de aplicación + datos demo reales
│   └── NexoPOS.IntegrationTests # Endpoints (WebApplicationFactory)
└── web
    └── nexopos.web              # Frontend React + TypeScript
```

Dirección de dependencias: `Domain ← Application ← Infrastructure ← API`.
El frontend se comunica por HTTP; no referencia proyectos .NET.

## Requisitos previos

- SDK de **.NET 10**
- **Node.js** 20+ y **npm**

## 1) Backend (API)

Desde la raíz del repositorio:

```bash
dotnet restore
```

```bash
dotnet build
```

```bash
dotnet test
```

Ejecutar el API en desarrollo (perfil **http**, escucha en `http://localhost:5257`):

```bash
dotnet run --project src/NexoPOS.API --launch-profile http
```

> Se usa el perfil `http` para que el frontend consuma el API sin redirección a
> HTTPS ni advertencias de certificado durante la demo.

## 2) Frontend (web)

En otra terminal, dentro de `web/nexopos.web`:

```bash
npm install
```

Copie el archivo de ejemplo de variables de entorno (opcional; el valor por
defecto ya apunta al API local):

```bash
cp .env.example .env.local
```

Levantar el servidor de desarrollo (Vite, `http://localhost:64351`):

```bash
npm run dev
```

Compilar para producción:

```bash
npm run build
```

Variable de entorno relevante:

| Variable            | Descripción                       | Valor por defecto        |
| ------------------- | --------------------------------- | ------------------------ |
| `VITE_API_BASE_URL` | URL base del API de NexoPOS       | `http://localhost:5257`  |

CORS está limitado al origen del frontend (`http://localhost:64351`) mediante
`Cors:AllowedOrigins` en `src/NexoPOS.API/appsettings.json` (no se habilita CORS
abierto a cualquier origen).

## Endpoints del API

| Método | Ruta                              | Descripción                                          |
| ------ | --------------------------------- | ----------------------------------------------------- |
| `GET`  | `/api/demo/overview`              | Indicadores, sucursales, alertas prioritarias y actividad |
| `GET`  | `/api/demo/inventory`             | Inventario consolidado (sucursales + botiquines)      |
| `GET`  | `/api/demo/branches/{id}`         | Detalle de una sucursal (404 si no existe)            |
| `GET`  | `/api/demo/products/{id}`         | Detalle ampliado de un producto (404 si no existe)    |
| `GET`  | `/api/demo/services`              | Catálogo de servicios y paquetes compuestos           |
| `GET`  | `/api/demo/mobile-kits`           | Botiquines móviles con contenido y actividad          |
| `GET`  | `/api/demo/invoices`              | Comprobantes de demostración (lista + indicadores)    |
| `GET`  | `/api/demo/invoices/{id}`         | Detalle de un comprobante (404 si no existe)          |
| `GET`  | `/api/demo/reorder?basis=weekly`  | Reposición sugerida (promedio semanal)                |
| `GET`  | `/api/demo/reorder?basis=monthly` | Reposición sugerida (promedio mensual)                |

Sucursales de ejemplo: `sanjose`, `heredia`, `cartago`.

## Rutas del frontend

| Ruta                     | Vista                                       |
| ------------------------ | -------------------------------------------- |
| `/`                      | Resumen general (dashboard)                  |
| `/sucursales`            | Listado de sucursales                        |
| `/sucursales/:branchId`  | Detalle de una sucursal                      |
| `/inventario`            | Inventario multi-sucursal con filtros        |
| `/servicios`             | Servicios y paquetes compuestos              |
| `/botiquines`            | Botiquines móviles                           |
| `/facturacion`           | Comprobantes de demostración (solo lectura)  |
| `/reposicion`            | Reposición sugerida por proveedor            |

## Limitaciones intencionales de esta maqueta

- Datos en memoria; sin base de datos ni persistencia.
- Sin autenticación real (usuario "Jefferson Rojas — Administrador general" es fijo).
- Módulos de Proveedores, Cuentas por cobrar/pagar, Cajas, Reportes, Usuarios y
  permisos, y Configuración aparecen como **"Próximamente"**.
- Clientes y mascotas, Expedientes clínicos, y Citas y vacunas aparecen como
  **"Por confirmar"** (no forman parte del alcance confirmado con el cliente).
- Las acciones de registro/transferencia/orden/emisión abren un modal rotulado
  como **demostración**; no modifican datos.
- Facturación es de solo lectura; no hay integración con Hacienda, XML, claves
  ni consecutivos fiscales reales. El IVA mostrado en el detalle de comprobante
  es un estimado ilustrativo (13%), no un cálculo fiscal oficial.
- El selector de ubicación del encabezado es informativo en esta iteración
  (no filtra todavía cada pantalla); el filtro de "Ubicación" del inventario sí
  es funcional.
