import type { ReactNode } from 'react';
import { Icon } from './Icon';
import type { IconName } from './Icon';

interface EmptyStateProps {
  message: string;
  icon?: IconName;
  action?: ReactNode;
}

/** Estado vacío reutilizable: se usa cuando una lista no tiene datos que mostrar. */
export function EmptyState({ message, icon = 'inbox', action }: EmptyStateProps) {
  return (
    <div className="nx-state">
      <span className="nx-state__icon">
        <Icon name={icon} size={22} />
      </span>
      <p className="nx-state__text">{message}</p>
      {action}
    </div>
  );
}
