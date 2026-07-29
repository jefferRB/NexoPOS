import { useEffect, useRef } from 'react';
import { Icon } from '../ui/Icon';
import { ActivityFeed } from '../dashboard/ActivityFeed';
import { EmptyState } from '../ui/EmptyState';
import { QuantityDisplay } from '../ui/QuantityDisplay';
import { formatCRC } from '../../utils/format';
import type { MobileKit } from '../../types/api';

interface KitContentModalProps {
  kit: MobileKit | null;
  onClose: () => void;
}

export function KitContentModal({ kit, onClose }: KitContentModalProps) {
  const closeRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    if (!kit) {
      return;
    }
    closeRef.current?.focus();
    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        onClose();
      }
    };
    document.addEventListener('keydown', onKeyDown);
    return () => document.removeEventListener('keydown', onKeyDown);
  }, [kit, onClose]);

  if (!kit) {
    return null;
  }

  return (
    <div className="nx-modal__backdrop" onClick={onClose}>
      <div
        className="nx-modal nx-modal--wide"
        role="dialog"
        aria-modal="true"
        aria-labelledby="nx-kit-title"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="nx-modal__header">
          <span className="nx-modal__icon">
            <Icon name="kit" size={22} />
          </span>
          <div>
            <h2 className="nx-modal__title" id="nx-kit-title">
              {kit.name}
            </h2>
            <span className="nx-tag">{kit.assignedTo} · {kit.homeBranchName}</span>
          </div>
        </div>
        <div className="nx-modal__body">
          <h3 className="nx-product-detail__heading">Contenido ({formatCRC(kit.estimatedValue)} estimado)</h3>
          {kit.stock.length > 0 ? (
            <div className="nx-component-list">
              {kit.stock.map((line) => (
                <div className="nx-component-row" key={line.productId}>
                  <span className="nx-component-row__label">{line.productName}</span>
                  <QuantityDisplay value={line.quantity} unit={line.unit} />
                </div>
              ))}
            </div>
          ) : (
            <EmptyState message="Este botiquín no tiene existencias registradas." />
          )}

          <h3 className="nx-product-detail__heading" style={{ marginTop: 18 }}>
            Actividad del botiquín
          </h3>
          {kit.recentActivity.length > 0 ? (
            <ActivityFeed items={kit.recentActivity} showLocation={false} />
          ) : (
            <EmptyState message="Sin movimientos recientes registrados para este botiquín." />
          )}
        </div>
        <div className="nx-modal__footer">
          <button ref={closeRef} type="button" className="nx-btn nx-btn--primary" onClick={onClose}>
            Cerrar
          </button>
        </div>
      </div>
    </div>
  );
}
