// Etiquetas en español para los códigos que expone el API. Centralizadas para
// no repetir el mismo mapa en cada página.
import type {
  InvoiceStatusCode,
  InvoiceTypeCode,
  MobileKitStatusCode,
  PaymentMethodCode,
  ProductTypeCode,
  ReorderPriorityCode,
  UnitCode,
} from '../types/api';

export const productTypeLabels: Record<ProductTypeCode, string> = {
  standard: 'Estándar',
  medication: 'Medicamento',
  food: 'Alimento',
  'clinical-supply': 'Insumo clínico',
};

export const unitLabels: Record<UnitCode, string> = {
  unit: 'Unidades',
  ml: 'Mililitros (ml)',
  kg: 'Kilogramos (kg)',
};

export const mobileKitStatusLabels: Record<MobileKitStatusCode, string> = {
  'on-route': 'En ruta',
  available: 'Disponible',
  'needs-review': 'Requiere revisión',
};

export const invoiceTypeLabels: Record<InvoiceTypeCode, string> = {
  'electronic-invoice': 'Factura electrónica',
  'electronic-ticket': 'Tiquete electrónico',
};

export const paymentMethodLabels: Record<PaymentMethodCode, string> = {
  cash: 'Efectivo',
  card: 'Tarjeta',
  'sinpe-movil': 'SINPE Móvil',
  'bank-transfer': 'Transferencia',
  credit: 'Crédito',
  mixed: 'Pago combinado',
};

export const invoiceStatusLabels: Record<InvoiceStatusCode, string> = {
  accepted: 'Aceptado',
  pending: 'Pendiente',
  voided: 'Anulado',
};

export const reorderPriorityLabels: Record<ReorderPriorityCode, string> = {
  high: 'Alta',
  medium: 'Media',
  low: 'Baja',
};
