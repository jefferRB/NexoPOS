import type { ReactNode } from 'react';
import { Icon } from './Icon';
import { EmptyState } from './EmptyState';

/**
 * Envuelve contenido dependiente del API y muestra los estados de carga, error
 * y vacío de forma consistente. Cuando hay datos, renderiza `children`.
 */
interface DataStateProps {
  loading: boolean;
  error: string | null;
  isEmpty?: boolean;
  onRetry?: () => void;
  loadingLabel?: string;
  emptyLabel?: string;
  children: ReactNode;
}

export function DataState({
  loading,
  error,
  isEmpty = false,
  onRetry,
  loadingLabel = 'Cargando datos…',
  emptyLabel = 'No hay información para mostrar.',
  children,
}: DataStateProps) {
  if (loading) {
    return (
      <div className="nx-state" role="status" aria-live="polite">
        <div className="nx-spinner" aria-hidden="true" />
        <p className="nx-state__text">{loadingLabel}</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="nx-state" role="alert">
        <span className="nx-state__icon" style={{ color: 'var(--nx-danger-text)', background: 'var(--nx-danger-bg)' }}>
          <Icon name="alert" size={24} />
        </span>
        <p className="nx-state__title">No se pudieron cargar los datos</p>
        <p className="nx-state__text">{error}</p>
        {onRetry && (
          <button type="button" className="nx-btn nx-btn--subtle" onClick={onRetry}>
            Reintentar
          </button>
        )}
      </div>
    );
  }

  if (isEmpty) {
    return <EmptyState message={emptyLabel} />;
  }

  return <>{children}</>;
}
