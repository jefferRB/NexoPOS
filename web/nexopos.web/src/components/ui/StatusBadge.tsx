import { mobileKitStatusLabels } from '../../utils/labels';
import type { MobileKitStatusCode, StockStatusCode } from '../../types/api';

const labels: Record<StockStatusCode, string> = {
  available: 'Disponible',
  low: 'Bajo',
  out: 'Agotado',
};

export function StatusBadge({ status }: { status: StockStatusCode }) {
  return (
    <span className={`nx-badge nx-badge--${status}`}>
      <span className="nx-dot" aria-hidden="true" />
      {labels[status]}
    </span>
  );
}

export function MobileKitStatusBadge({ status }: { status: MobileKitStatusCode }) {
  return (
    <span className={`nx-badge nx-badge--${status}`}>
      <span className="nx-dot" aria-hidden="true" />
      {mobileKitStatusLabels[status]}
    </span>
  );
}

export function OperationalBadge({ isOperational }: { isOperational: boolean }) {
  return isOperational ? (
    <span className="nx-badge nx-badge--available">
      <span className="nx-dot" aria-hidden="true" />
      Operativa
    </span>
  ) : (
    <span className="nx-badge nx-badge--neutral">
      <span className="nx-dot" aria-hidden="true" />
      Cerrada
    </span>
  );
}
