import { Icon } from '../ui/Icon';
import type { IconName } from '../ui/Icon';
import type { AlertCategory, AlertSeverity, PriorityAlert } from '../../types/api';

const iconByCategory: Record<AlertCategory, IconName> = {
  'stock-low': 'alert',
  'stock-out': 'alert',
  'mobile-kit': 'kit',
  transfer: 'transfer',
  reorder: 'reorder',
};

const toneClass: Record<AlertSeverity, string> = {
  high: 'nx-priority-alert__icon--high',
  medium: 'nx-priority-alert__icon--medium',
  low: 'nx-priority-alert__icon--low',
};

export function PriorityAlerts({ alerts }: { alerts: PriorityAlert[] }) {
  return (
    <ul className="nx-priority-alerts" aria-label="Alertas prioritarias">
      {alerts.map((alert) => (
        <li className="nx-priority-alert" key={alert.id}>
          <span className={`nx-priority-alert__icon ${toneClass[alert.severity]}`} aria-hidden="true">
            <Icon name={iconByCategory[alert.category]} size={17} />
          </span>
          <div className="nx-priority-alert__body">
            <span className="nx-priority-alert__title">{alert.title}</span>
            <span className="nx-priority-alert__desc">{alert.description}</span>
          </div>
        </li>
      ))}
    </ul>
  );
}
