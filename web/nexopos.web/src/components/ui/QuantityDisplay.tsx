import { formatQuantity } from '../../utils/format';
import type { UnitCode } from '../../types/api';

interface QuantityDisplayProps {
  value: number;
  unit: UnitCode;
  className?: string;
}

/**
 * Muestra una cantidad junto a su unidad ("850 ml", "12.5 kg", "24 unidades").
 * Centraliza el formato para no mezclar unidades incompatibles en ningún lugar.
 */
export function QuantityDisplay({ value, unit, className }: QuantityDisplayProps) {
  return <span className={className}>{formatQuantity(value, unit)}</span>;
}
