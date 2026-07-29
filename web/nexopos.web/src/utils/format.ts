// Formato de moneda y fechas para Costa Rica. Se centraliza aquí para no repetir
// lógica de formato en cada componente.

const TIME_ZONE = 'America/Costa_Rica';

const currencyFormatter = new Intl.NumberFormat('es-CR', {
  style: 'currency',
  currency: 'CRC',
  maximumFractionDigits: 0,
});

const numberFormatter = new Intl.NumberFormat('es-CR');

const decimalFormatter = new Intl.NumberFormat('es-CR', { maximumFractionDigits: 2 });

const dateTimeFormatter = new Intl.DateTimeFormat('es-CR', {
  timeZone: TIME_ZONE,
  day: '2-digit',
  month: 'short',
  hour: '2-digit',
  minute: '2-digit',
  hour12: true,
});

const shortDateFormatter = new Intl.DateTimeFormat('es-CR', {
  timeZone: TIME_ZONE,
  weekday: 'short',
  day: '2-digit',
});

const unitLabels: Record<string, { singular: string; plural: string }> = {
  unit: { singular: 'unidad', plural: 'unidades' },
  ml: { singular: 'ml', plural: 'ml' },
  kg: { singular: 'kg', plural: 'kg' },
};

/** Formatea un monto en colones costarricenses (CRC). */
export function formatCRC(amount: number): string {
  return currencyFormatter.format(amount);
}

/** Formatea una cantidad entera con separadores de miles. */
export function formatNumber(value: number): string {
  return numberFormatter.format(value);
}

/** Formatea una cantidad con hasta dos decimales (para kilogramos y mililitros fraccionados). */
export function formatDecimal(value: number): string {
  return decimalFormatter.format(value);
}

/**
 * Formatea una cantidad con su unidad, sin mezclar unidades incompatibles:
 * "850 ml", "12.5 kg", "24 unidades".
 */
export function formatQuantity(value: number, unit: string): string {
  const labels = unitLabels[unit] ?? { singular: unit, plural: unit };
  const label = value === 1 ? labels.singular : labels.plural;
  return `${formatDecimal(value)} ${label}`;
}

/** Formatea una fecha ISO 8601 a fecha y hora de Costa Rica. */
export function formatDateTime(isoDate: string): string {
  const date = new Date(isoDate);
  if (Number.isNaN(date.getTime())) {
    return isoDate;
  }
  return dateTimeFormatter.format(date);
}

/**
 * Tasa de IVA de Costa Rica, usada únicamente para mostrar un estimado
 * demostrativo en el detalle de comprobantes. El API no modela impuestos
 * todavía; esto no representa el cálculo fiscal oficial.
 */
const DEMO_IVA_RATE = 0.13;

/** Estima el IVA incluido en un monto ya facturado (impuesto demostrativo, no oficial). */
export function estimateIncludedTax(total: number): number {
  return total - total / (1 + DEMO_IVA_RATE);
}

/** Formatea una fecha corta (para etiquetas de gráficos), p. ej. "lun. 27". */
export function formatShortDate(isoDate: string): string {
  const date = new Date(`${isoDate}T00:00:00`);
  if (Number.isNaN(date.getTime())) {
    return isoDate;
  }
  return shortDateFormatter.format(date);
}

/** Devuelve una descripción relativa breve ("hace 5 min", "hace 2 h"). */
export function formatRelative(isoDate: string): string {
  const date = new Date(isoDate);
  if (Number.isNaN(date.getTime())) {
    return '';
  }

  const diffMs = Date.now() - date.getTime();
  const diffMinutes = Math.round(diffMs / 60000);

  if (diffMinutes < 1) {
    return 'hace instantes';
  }
  if (diffMinutes < 60) {
    return `hace ${diffMinutes} min`;
  }

  const diffHours = Math.round(diffMinutes / 60);
  if (diffHours < 24) {
    return `hace ${diffHours} h`;
  }

  const diffDays = Math.round(diffHours / 24);
  return `hace ${diffDays} d`;
}
