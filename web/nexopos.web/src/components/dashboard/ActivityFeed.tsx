import { Icon } from '../ui/Icon';
import type { IconName } from '../ui/Icon';
import { LocationBadge } from '../ui/LocationBadge';
import { formatCRC, formatDateTime, formatRelative } from '../../utils/format';
import type { ActivityItem, ActivityTypeCode } from '../../types/api';

const iconByType: Record<ActivityTypeCode, IconName> = {
  'invoice-issued': 'invoice',
  'fractional-sale': 'sale',
  'service-billed': 'stethoscope',
  adjustment: 'adjust',
  purchase: 'receive',
  'transfer-out': 'transfer',
  'transfer-in': 'box',
  'transfer-kit': 'kit',
  'receivable-payment': 'receivable',
  'cash-closing': 'cash',
};

const variantByType: Record<ActivityTypeCode, string> = {
  'invoice-issued': 'sale',
  'fractional-sale': 'sale',
  'service-billed': 'sale',
  adjustment: 'adjustment',
  purchase: 'purchase',
  'transfer-out': 'transfer',
  'transfer-in': 'transfer',
  'transfer-kit': 'transfer',
  'receivable-payment': 'purchase',
  'cash-closing': 'adjustment',
};

interface ActivityFeedProps {
  items: ActivityItem[];
  showLocation?: boolean;
}

export function ActivityFeed({ items, showLocation = true }: ActivityFeedProps) {
  return (
    <ul className="nx-activity" aria-label="Actividad reciente">
      {items.map((item) => (
        <li className="nx-activity__item" key={item.id}>
          <span className={`nx-activity__icon nx-activity__icon--${variantByType[item.type]}`} aria-hidden="true">
            <Icon name={iconByType[item.type]} size={18} />
          </span>
          <div className="nx-activity__body">
            <div className="nx-activity__title">
              <span className="nx-activity__action">{item.action}</span>
              {item.amount != null && (
                <span className="nx-activity__amount">{formatCRC(item.amount)}</span>
              )}
            </div>
            <div className="nx-activity__meta">
              <span>
                <b>{item.userName}</b> · {item.userRole}
              </span>
              {showLocation && (
                <LocationBadge name={item.locationName} isMobileKit={item.mobileKitId != null} />
              )}
              <span>{item.reference}</span>
              <span>
                <time dateTime={item.timestamp} title={formatDateTime(item.timestamp)}>{formatRelative(item.timestamp)}</time>
              </span>
            </div>
            <div className="nx-activity__reason">{item.reason}</div>
          </div>
        </li>
      ))}
    </ul>
  );
}
