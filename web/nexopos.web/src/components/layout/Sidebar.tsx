import { useState } from 'react';
import { Icon } from '../ui/Icon';
import { DemoModal } from '../ui/DemoModal';
import type { DemoAction } from '../ui/DemoModal';
import { Link, useLocation } from '../../router/router';
import { navSections } from './navigation';
import type { NavItem } from './navigation';
import { comingSoonContent } from './comingSoonContent';

interface SidebarProps {
  onNavigate?: () => void;
}

export function Sidebar({ onNavigate }: SidebarProps) {
  const path = useLocation();
  const [comingSoon, setComingSoon] = useState<DemoAction | null>(null);

  const openComingSoon = (item: NavItem) => {
    const info = comingSoonContent[item.label];
    setComingSoon({
      title: item.label,
      icon: item.icon,
      description: info?.description ?? 'Este módulo estará disponible en una próxima iteración.',
      points: info?.points ?? [],
    });
  };

  return (
    <>
      <div className="nx-sidebar__brand">
        <span className="nx-logo" aria-hidden="true">
          NX
        </span>
        <span className="nx-brand__text">
          <span className="nx-brand__name">NexoPOS Veterinarias</span>
          <span className="nx-brand__company">Grupo Veterinario Demo</span>
        </span>
      </div>

      <nav className="nx-nav" aria-label="Navegación principal">
        {navSections.map((section) => (
          <div className="nx-nav__section" key={section.label}>
            <span className="nx-nav__label">{section.label}</span>
            {section.items.map((item) => (
              <NavRow
                key={item.label}
                item={item}
                path={path}
                onNavigate={onNavigate}
                onSoonClick={openComingSoon}
              />
            ))}
          </div>
        ))}
      </nav>

      <div className="nx-sidebar__footer">
        <div className="nx-demo-indicator">
          <span className="nx-pulse" aria-hidden="true" />
          <span className="nx-demo-indicator__text">Datos de demostración</span>
        </div>
      </div>

      <DemoModal action={comingSoon} onClose={() => setComingSoon(null)} />
    </>
  );
}

function NavRow({
  item,
  path,
  onNavigate,
  onSoonClick,
}: {
  item: NavItem;
  path: string;
  onNavigate?: () => void;
  onSoonClick: (item: NavItem) => void;
}) {
  if (item.status || !item.path) {
    const badgeText = item.status === 'unconfirmed' ? 'Por confirmar' : 'Próximamente';
    return (
      <button
        type="button"
        className={`nx-nav__item nx-nav__item--soon nx-nav__item--${item.status ?? 'soon'}`}
        onClick={() => onSoonClick(item)}
      >
        <span className="nx-nav__icon">
          <Icon name={item.icon} />
        </span>
        <span className="nx-nav__text">{item.label}</span>
        <span className="nx-nav__soon">{badgeText}</span>
      </button>
    );
  }

  const active = item.match ? item.match(path) : path === item.path;
  return (
    <Link
      to={item.path}
      className={`nx-nav__item${active ? ' nx-nav__item--active' : ''}`}
      aria-current={active ? 'page' : undefined}
      onClick={onNavigate}
    >
      <span className="nx-nav__icon">
        <Icon name={item.icon} />
      </span>
      <span className="nx-nav__text">{item.label}</span>
    </Link>
  );
}
