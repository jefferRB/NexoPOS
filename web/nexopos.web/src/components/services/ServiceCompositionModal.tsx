import { useEffect, useRef } from 'react';
import { Icon } from '../ui/Icon';
import { formatCRC, formatQuantity } from '../../utils/format';
import type { Service } from '../../types/api';

interface ServiceCompositionModalProps {
  service: Service | null;
  onClose: () => void;
}

export function ServiceCompositionModal({ service, onClose }: ServiceCompositionModalProps) {
  const closeRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    if (!service) {
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
  }, [service, onClose]);

  if (!service) {
    return null;
  }

  return (
    <div className="nx-modal__backdrop" onClick={onClose}>
      <div
        className="nx-modal nx-modal--wide"
        role="dialog"
        aria-modal="true"
        aria-labelledby="nx-service-title"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="nx-modal__header">
          <span className="nx-modal__icon">
            <Icon name="stethoscope" size={22} />
          </span>
          <div>
            <h2 className="nx-modal__title" id="nx-service-title">
              {service.name}
            </h2>
            <span className="nx-tag">{service.durationMinutes} min · {formatCRC(service.price)}</span>
          </div>
        </div>
        <div className="nx-modal__body">
          <p>{service.description}</p>

          <div className="nx-component-list">
            {service.components.map((component) => (
              <div className="nx-component-row" key={component.label}>
                <span className="nx-component-row__label">
                  <Icon name={component.linksToInventory ? 'inventory' : 'checklist'} size={15} />
                  {component.label}
                </span>
                <span>
                  {component.durationMinutes != null
                    ? `${component.durationMinutes} min`
                    : component.quantity != null && component.unit
                      ? formatQuantity(component.quantity, component.unit)
                      : 'Insumo genérico'}
                </span>
              </div>
            ))}
          </div>

          <p className="nx-modal__footnote">
            Los componentes marcados con <Icon name="inventory" size={12} /> descuentan inventario real al
            facturar el servicio. Los insumos genéricos y el tiempo de veterinario son informativos.
          </p>
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
