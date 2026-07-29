export interface ComingSoonInfo {
  description: string;
  points: string[];
}

/** Contenido informativo para los módulos marcados "Próximamente" o "Por confirmar" en la navegación. */
export const comingSoonContent: Record<string, ComingSoonInfo> = {
  Proveedores: {
    description: 'Administrará el directorio de proveedores usado por el inventario y las órdenes de reposición.',
    points: ['Datos de contacto y condiciones de compra', 'Historial de órdenes por proveedor', 'Vínculo directo con Reposición'],
  },
  'Cuentas por cobrar': {
    description: 'Llevará el detalle de saldos pendientes por cliente, con abonos y vencimientos.',
    points: ['Saldo por cliente', 'Registro de abonos', 'Alertas de mora'],
  },
  'Cuentas por pagar': {
    description: 'Llevará el detalle de saldos pendientes con proveedores y sus fechas de pago.',
    points: ['Saldo por proveedor', 'Registro de pagos', 'Calendario de vencimientos'],
  },
  Cajas: {
    description: 'Controlará la apertura, arqueo y cierre de caja por sucursal y turno.',
    points: ['Apertura y cierre de turno', 'Arqueo por método de pago', 'Diferencias por justificar'],
  },
  Reportes: {
    description: 'Centralizará los reportes operativos y financieros de las tres veterinarias, exportables a Excel.',
    points: [
      'Cierre diario, semanal y mensual',
      'Reporte por rango de fechas',
      'Ventas por sucursal',
      'Productos más vendidos',
      'Histórico y estacionalidad',
      'Inventario, cuentas por cobrar y cuentas por pagar',
      'Reposición sugerida',
      'Movimientos por usuario',
      'Exportación a Excel',
    ],
  },
  'Usuarios y permisos': {
    description: 'Permitirá crear usuarios y asignar permisos por módulo y por sucursal.',
    points: ['Roles predefinidos', 'Permisos por módulo', 'Acceso restringido por sucursal'],
  },
  Configuración: {
    description: 'Reunirá los ajustes generales de la empresa, sucursales e impuestos.',
    points: ['Datos de la empresa', 'Parámetros de facturación', 'Preferencias del sistema'],
  },
  'Clientes y mascotas': {
    description: 'Aún por confirmar con el cliente: un directorio de dueños y sus mascotas.',
    points: ['Datos de contacto del dueño', 'Ficha básica de la mascota', 'Historial de visitas'],
  },
  'Expedientes clínicos': {
    description: 'Aún por confirmar con el cliente: el expediente clínico completo de cada mascota.',
    points: ['Diagnósticos y tratamientos', 'Adjuntos de exámenes', 'Historial médico'],
  },
  'Citas y vacunas': {
    description: 'Aún por confirmar con el cliente: agenda de citas y control de esquema de vacunación.',
    points: ['Calendario de citas', 'Recordatorios de vacunas', 'Confirmación por sucursal'],
  },
};
