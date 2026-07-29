import type { StockStatusCode } from '../types/api';

/**
 * Misma regla de negocio que el dominio (StockStatusRules.Evaluate): permite
 * evaluar el estado por sucursal en el cliente al aplicar el filtro de sucursal.
 */
export function evaluateStatus(quantity: number, minimum: number): StockStatusCode {
  if (quantity <= 0) {
    return 'out';
  }
  return quantity <= minimum ? 'low' : 'available';
}
