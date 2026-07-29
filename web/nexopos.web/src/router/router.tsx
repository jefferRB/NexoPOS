import { useCallback, useEffect, useSyncExternalStore } from 'react';
import type { AnchorHTMLAttributes, MouseEvent, ReactNode } from 'react';

// Enrutador mínimo basado en la History API. Cubre las pocas rutas de la maqueta
// sin añadir dependencias externas.

const NAVIGATION_EVENT = 'nx:navigation';

export function navigate(to: string): void {
  if (to === currentPath()) {
    return;
  }
  window.history.pushState({}, '', to);
  window.dispatchEvent(new Event(NAVIGATION_EVENT));
}

function currentPath(): string {
  return window.location.pathname || '/';
}

function subscribe(callback: () => void): () => void {
  window.addEventListener('popstate', callback);
  window.addEventListener(NAVIGATION_EVENT, callback);
  return () => {
    window.removeEventListener('popstate', callback);
    window.removeEventListener(NAVIGATION_EVENT, callback);
  };
}

/** Devuelve la ruta actual y se re-renderiza cuando cambia. */
export function useLocation(): string {
  return useSyncExternalStore(subscribe, currentPath, () => '/');
}

/** Lleva el foco y el scroll al inicio cuando cambia la ruta (accesibilidad). */
export function useScrollToTopOnNavigate(path: string): void {
  useEffect(() => {
    window.scrollTo({ top: 0 });
  }, [path]);
}

interface LinkProps extends Omit<AnchorHTMLAttributes<HTMLAnchorElement>, 'href'> {
  to: string;
  children: ReactNode;
}

export function Link({ to, children, onClick, ...rest }: LinkProps) {
  const handleClick = useCallback(
    (event: MouseEvent<HTMLAnchorElement>) => {
      onClick?.(event);
      // Respeta clics con modificadores o botón central (abrir en pestaña nueva).
      if (
        event.defaultPrevented ||
        event.button !== 0 ||
        event.metaKey ||
        event.ctrlKey ||
        event.shiftKey ||
        event.altKey
      ) {
        return;
      }
      event.preventDefault();
      navigate(to);
    },
    [onClick, to],
  );

  return (
    <a href={to} onClick={handleClick} {...rest}>
      {children}
    </a>
  );
}
