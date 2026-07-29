import { useEffect, useState } from 'react';
import type { ReactNode } from 'react';
import { Sidebar } from './Sidebar';
import { Topbar } from './Topbar';
import { useMediaQuery } from '../../hooks/useMediaQuery';
import { useLocation } from '../../router/router';

interface AppShellProps {
  title: string;
  children: ReactNode;
}

/**
 * Estructura general: barra lateral colapsable + encabezado + contenido.
 * En escritorio la barra se colapsa a modo "riel"; en tablet se comporta como
 * un panel deslizante (drawer) con fondo oscuro.
 */
export function AppShell({ title, children }: AppShellProps) {
  const isCompact = useMediaQuery('(max-width: 1024px)');
  const path = useLocation();
  const [collapsed, setCollapsed] = useState(false);
  const [mobileOpen, setMobileOpen] = useState(false);

  // Cierra el drawer al cambiar de ruta (en tablet/móvil).
  useEffect(() => {
    setMobileOpen(false);
  }, [path]);

  const toggleNav = () => {
    if (isCompact) {
      setMobileOpen((open) => !open);
    } else {
      setCollapsed((value) => !value);
    }
  };

  const shellClass = `nx-shell${collapsed && !isCompact ? ' nx-shell--collapsed' : ''}`;
  const sidebarClass = `nx-sidebar${mobileOpen ? ' nx-sidebar--open' : ''}`;

  return (
    <div className={shellClass}>
      <aside className={sidebarClass}>
        <Sidebar onNavigate={() => setMobileOpen(false)} />
      </aside>

      {isCompact && mobileOpen && (
        <div className="nx-backdrop" onClick={() => setMobileOpen(false)} aria-hidden="true" />
      )}

      <div className="nx-main">
        <Topbar title={title} onToggleNav={toggleNav} />
        <main className="nx-content" id="contenido" tabIndex={-1}>
          {children}
        </main>
      </div>
    </div>
  );
}
