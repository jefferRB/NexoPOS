import { Icon } from './Icon';
import type { IconName } from './Icon';

export type StatTone = 'primary' | 'ok' | 'warn' | 'info';

interface StatCardProps {
  label: string;
  value: string;
  icon: IconName;
  tone?: StatTone;
  caption?: string;
}

const toneClass: Record<StatTone, string> = {
  primary: '',
  ok: ' nx-stat__icon--ok',
  warn: ' nx-stat__icon--warn',
  info: ' nx-stat__icon--info',
};

export function StatCard({ label, value, icon, tone = 'primary', caption }: StatCardProps) {
  return (
    <article className="nx-stat">
      <div className="nx-stat__top">
        <span className="nx-stat__label">{label}</span>
        <span className={`nx-stat__icon${toneClass[tone]}`} aria-hidden="true">
          <Icon name={icon} />
        </span>
      </div>
      <div className="nx-stat__value">{value}</div>
      {caption && <div className="nx-stat__caption">{caption}</div>}
    </article>
  );
}
