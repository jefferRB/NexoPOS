import { useEffect, useRef } from 'react';
import { Icon } from '../ui/Icon';

const PERMISSIONS = [
  'Ver todas las sucursales',
  'Registrar ventas',
  'Autorizar ajustes',
  'Crear órdenes',
  'Consultar reportes',
  'Administrar usuarios',
];

interface PermissionsModalProps {
  open: boolean;
  onClose: () => void;
}

export function PermissionsModal({ open, onClose }: PermissionsModalProps) {
  const closeRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    if (!open) {
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
  }, [open, onClose]);

  if (!open) {
    return null;
  }

  return (
    <div className="nx-modal__backdrop" onClick={onClose}>
      <div
        className="nx-modal"
        role="dialog"
        aria-modal="true"
        aria-labelledby="nx-permissions-title"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="nx-modal__header">
          <span className="nx-modal__icon">
            <Icon name="shield" size={22} />
          </span>
          <div>
            <h2 className="nx-modal__title" id="nx-permissions-title">
              Permisos del usuario
            </h2>
            <span className="nx-tag">Jefferson Rojas · Administrador general</span>
          </div>
        </div>
        <div className="nx-modal__body">
          <ul className="nx-modal__list">
            {PERMISSIONS.map((permission) => (
              <li key={permission}>{permission}</li>
            ))}
          </ul>
          <p className="nx-modal__footnote">
            Cada usuario podrá tener acceso únicamente a los módulos y sucursales autorizados.
          </p>
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
