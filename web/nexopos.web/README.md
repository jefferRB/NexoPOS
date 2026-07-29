# NexoPOS · Frontend (web)

Interfaz de la maqueta de NexoPOS (React 19 + TypeScript + Vite). Consume el API
de demostración por HTTP.

## Comandos

```bash
npm install
```

```bash
npm run dev
```

```bash
npm run build
```

## Configuración

- `VITE_API_BASE_URL` — URL base del API. Por defecto `http://localhost:5257`.
  Copie `.env.example` a `.env.local` para ajustarla.

## Organización

```
src/
├── api/         # Cliente HTTP centralizado y llamadas al API
├── components/  # layout, dashboard, inventory, services, mobilekits, invoicing, reorder y UI reutilizable
├── features/    # Páginas: dashboard, branches, inventory, services, mobilekits, invoicing, reorder
├── hooks/       # useApiData, useMediaQuery
├── router/      # Enrutador mínimo (History API)
├── styles/      # Sistema de diseño (global.css)
├── types/       # Tipos de las respuestas del API
└── utils/       # Formato de moneda (CRC), fechas (CR), unidades, etiquetas y CSV
```

Consulte el `README.md` en la raíz del repositorio para el flujo completo
(backend + frontend).
