import { useEffect, useRef } from 'react';
import { Icon } from './Icon';
import type { IconName } from './Icon';

export interface DemoAction {
  title: string;
  icon: IconName;
  description: string;
  points: string[];
}

interface DemoModalProps {
  action: DemoAction | null;
  onClose: () => void;
}

/**
 * Modal accesible para las acciones de demostración. Deja claro que la función
 * definitiva aún no está implementada; no modifica ningún dato.
 */
export function DemoModal({ action, onClose }: DemoModalProps) {
  const closeRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    if (!action) {
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
  }, [action, onClose]);

  if (!action) {
    return null;
  }

  return (
    <div className="nx-modal__backdrop" onClick={onClose}>
      <div
        className="nx-modal"
        role="dialog"
        aria-modal="true"
        aria-labelledby="nx-demo-title"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="nx-modal__header">
          <span className="nx-modal__icon">
            <Icon name={action.icon} size={22} />
          </span>
          <div>
            <h2 className="nx-modal__title" id="nx-demo-title">
              {action.title}
            </h2>
            <span className="nx-demo-tag">
              <Icon name="alert" size={12} /> Demostración
            </span>
          </div>
        </div>
        <div className="nx-modal__body">
          <p>{action.description}</p>
          <ul className="nx-modal__list">
            {action.points.map((point) => (
              <li key={point}>{point}</li>
            ))}
          </ul>
        </div>
        <div className="nx-modal__footer">
          <button ref={closeRef} type="button" className="nx-btn nx-btn--primary" onClick={onClose}>
            Entendido
          </button>
        </div>
      </div>
    </div>
  );
}
