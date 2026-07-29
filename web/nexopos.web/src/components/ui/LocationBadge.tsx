import { Icon } from './Icon';

interface LocationBadgeProps {
  name: string;
  isMobileKit?: boolean;
}

/** Indica si un movimiento o renglón pertenece a una sucursal o a un botiquín móvil. */
export function LocationBadge({ name, isMobileKit = false }: LocationBadgeProps) {
  return (
    <span className="nx-location-badge">
      <Icon name={isMobileKit ? 'kit' : 'pin'} size={13} />
      {name}
    </span>
  );
}
