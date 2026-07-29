import type { IconName } from '../ui/Icon';

export type NavStatus = 'soon' | 'unconfirmed';

export interface NavItem {
  label: string;
  icon: IconName;
  path?: string;
  status?: NavStatus;
  /** Coincidencia por prefijo para marcar el ítem como activo. */
  match?: (path: string) => boolean;
}

export interface NavSection {
  label: string;
  items: NavItem[];
}

export const navSections: NavSection[] = [
  {
    label: 'Operación',
    items: [
      { label: 'Resumen', icon: 'dashboard', path: '/', match: (p) => p === '/' },
      { label: 'Sucursales', icon: 'branch', path: '/sucursales', match: (p) => p.startsWith('/sucursales') },
      { label: 'Inventario', icon: 'inventory', path: '/inventario', match: (p) => p.startsWith('/inventario') },
      { label: 'Servicios', icon: 'stethoscope', path: '/servicios', match: (p) => p.startsWith('/servicios') },
      { label: 'Botiquines móviles', icon: 'kit', path: '/botiquines', match: (p) => p.startsWith('/botiquines') },
      { label: 'Facturación', icon: 'invoice', path: '/facturacion', match: (p) => p.startsWith('/facturacion') },
      { label: 'Reposición', icon: 'reorder', path: '/reposicion', match: (p) => p.startsWith('/reposicion') },
    ],
  },
  {
    label: 'Administración',
    items: [
      { label: 'Proveedores', icon: 'supplier', status: 'soon' },
      { label: 'Cuentas por cobrar', icon: 'receivable', status: 'soon' },
      { label: 'Cuentas por pagar', icon: 'payable', status: 'soon' },
      { label: 'Cajas', icon: 'cash', status: 'soon' },
      { label: 'Reportes', icon: 'report', status: 'soon' },
      { label: 'Usuarios y permisos', icon: 'users', status: 'soon' },
      { label: 'Configuración', icon: 'settings', status: 'soon' },
    ],
  },
  {
    label: 'Clínica',
    items: [
      { label: 'Clientes y mascotas', icon: 'pet', status: 'unconfirmed' },
      { label: 'Expedientes clínicos', icon: 'file', status: 'unconfirmed' },
      { label: 'Citas y vacunas', icon: 'calendar', status: 'unconfirmed' },
    ],
  },
];
