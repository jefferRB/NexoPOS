import { useState } from 'react';
import { Icon } from '../ui/Icon';
import { LocationSelector } from './LocationSelector';
import { PermissionsModal } from './PermissionsModal';

interface TopbarProps {
  title: string;
  onToggleNav: () => void;
}

export function Topbar({ title, onToggleNav }: TopbarProps) {
  const [permissionsOpen, setPermissionsOpen] = useState(false);

  return (
    <header className="nx-topbar">
      <button
        type="button"
        className="nx-topbar__toggle"
        onClick={onToggleNav}
        aria-label="Mostrar u ocultar el menú"
      >
        <Icon name="menu" />
      </button>

      <h1 className="nx-topbar__title">{title}</h1>

      <LocationSelector />

      <div className="nx-topbar__spacer" />

      <span className="nx-demo-pill">
        <span className="nx-dot" aria-hidden="true" />
        <span className="nx-demo-pill__text">Datos de demostración</span>
      </span>

      <button
        type="button"
        className="nx-user"
        onClick={() => setPermissionsOpen(true)}
        aria-haspopup="dialog"
      >
        <span className="nx-user__avatar" aria-hidden="true">
          JR
        </span>
        <span className="nx-user__meta">
          <span className="nx-user__name">Jefferson Rojas</span>
          <span className="nx-user__role">Administrador general</span>
        </span>
      </button>

      <PermissionsModal open={permissionsOpen} onClose={() => setPermissionsOpen(false)} />
    </header>
  );
}
