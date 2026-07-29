import type { CSSProperties } from 'react';

// Iconos SVG en línea (sin dependencias externas ni imágenes remotas).
// Trazos de 1.8px sobre una grilla de 24×24.

export type IconName =
  | 'dashboard'
  | 'branch'
  | 'inventory'
  | 'pos'
  | 'invoice'
  | 'supplier'
  | 'cash'
  | 'report'
  | 'settings'
  | 'menu'
  | 'search'
  | 'alert'
  | 'arrow-left'
  | 'plus'
  | 'transfer'
  | 'box'
  | 'sale'
  | 'adjust'
  | 'receive'
  | 'pin'
  | 'users'
  | 'chevron-right'
  | 'chevron-down'
  | 'inbox'
  | 'stethoscope'
  | 'kit'
  | 'reorder'
  | 'receivable'
  | 'payable'
  | 'pet'
  | 'file'
  | 'calendar'
  | 'shield'
  | 'download'
  | 'route'
  | 'wallet'
  | 'checklist'
  | 'phone';

const paths: Record<IconName, string> = {
  dashboard: 'M4 13h6V4H4v9Zm0 7h6v-5H4v5Zm10 0h6v-9h-6v9Zm0-16v5h6V4h-6Z',
  branch: 'M3 21h18M5 21V7l7-4 7 4v14M9 21v-5h6v5M9 10h.01M15 10h.01M9 13h.01M15 13h.01',
  inventory: 'M3 7l9-4 9 4-9 4-9-4Zm0 0v10l9 4 9-4V7M12 11v10',
  pos: 'M4 7h16v10H4zM4 11h16M8 15h4M9 3h6l1 4H8z',
  invoice: 'M6 2h9l3 3v17l-3-2-2 2-2-2-2 2-2-2-2 2V4a2 2 0 0 1 1-2ZM9 8h6M9 12h6M9 16h4',
  supplier: 'M3 13V6h11v7M14 9h4l3 3v3h-7M3 13h11M7 18a2 2 0 1 0 0 .01M17 18a2 2 0 1 0 0 .01',
  cash: 'M2 6h20v12H2zM12 9a3 3 0 1 0 0 6 3 3 0 0 0 0-6ZM5 9v.01M19 15v.01',
  report: 'M5 3h14a1 1 0 0 1 1 1v16a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1Zm3 13v-4m4 4V8m4 8v-6',
  settings:
    'M12 15a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm7.4-3a7.4 7.4 0 0 0-.13-1.3l2.05-1.6-2-3.46-2.42.98a7.3 7.3 0 0 0-2.24-1.3L14.3 2h-4l-.36 2.52c-.8.3-1.55.74-2.24 1.3L5.28 4.84l-2 3.46 2.05 1.6a7.4 7.4 0 0 0 0 2.6l-2.05 1.6 2 3.46 2.42-.98c.69.56 1.44 1 2.24 1.3L10.3 22h4l.36-2.52c.8-.3 1.55-.74 2.24-1.3l2.42.98 2-3.46-2.05-1.6c.09-.42.13-.86.13-1.3Z',
  menu: 'M4 6h16M4 12h16M4 18h16',
  search: 'M11 19a8 8 0 1 0 0-16 8 8 0 0 0 0 16Zm10 2-4.35-4.35',
  alert: 'M12 3 2 20h20L12 3Zm0 6v5m0 3v.01',
  'arrow-left': 'M19 12H5m0 0 6 6m-6-6 6-6',
  plus: 'M12 5v14M5 12h14',
  transfer: 'M4 8h13m0 0-4-4m4 4-4 4M20 16H7m0 0 4-4m-4 4 4 4',
  box: 'M3 7l9-4 9 4-9 4-9-4Zm0 0v10l9 4 9-4V7',
  sale: 'M3 3h2l2.4 12.4a1 1 0 0 0 1 .8h9.2a1 1 0 0 0 1-.8L21 7H6M9 21a1 1 0 1 0 0 .01M18 21a1 1 0 1 0 0 .01',
  adjust: 'M4 6h10M18 6h2M4 12h2M10 12h10M4 18h8M16 18h4M14 4v4M8 10v4M12 16v4',
  receive: 'M12 3v10m0 0 4-4m-4 4-4-4M4 15v4a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1v-4',
  pin: 'M12 21s7-6.5 7-11a7 7 0 1 0-14 0c0 4.5 7 11 7 11Zm0-8a3 3 0 1 0 0-6 3 3 0 0 0 0 6Z',
  users:
    'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm13 10v-2a4 4 0 0 0-3-3.87M16 3.13A4 4 0 0 1 16 11',
  'chevron-right': 'M9 6l6 6-6 6',
  'chevron-down': 'M6 9l6 6 6-6',
  inbox:
    'M22 12h-6l-2 3h-4l-2-3H2M5.5 5h13a2 2 0 0 1 1.8 1.1L22 12v6a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-6L3.7 6.1A2 2 0 0 1 5.5 5Z',
  stethoscope:
    'M5 3v6a4 4 0 0 0 8 0V3M9 21a5 5 0 0 0 5-5v-3.5M19 9v3a5 5 0 0 1-5 5M19 6a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3Z',
  kit: 'M4 8h16v11a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V8ZM8 8V6a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2M12 11v6M9 14h6',
  reorder: 'M4 4v6h6M20 20v-6h-6M4.5 15a8 8 0 0 0 14.4 3M19.5 9a8 8 0 0 0-14.4-3',
  receivable: 'M12 3v18M17 7.5c0-1.9-2.2-3.5-5-3.5s-5 1.4-5 3.5S9.2 11 12 11s5 1.6 5 3.5-2.2 3.5-5 3.5-5-1.6-5-3.5',
  payable: 'M3 10h18M3 6h18v13a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V6ZM7 15h4',
  pet: 'M9 8a2 2 0 1 0 0-4 2 2 0 0 0 0 4Zm6 0a2 2 0 1 0 0-4 2 2 0 0 0 0 4ZM5 12a2 2 0 1 0 0-4 2 2 0 0 0 0 4Zm14 0a2 2 0 1 0 0-4 2 2 0 0 0 0 4ZM12 21c-3 0-5.5-1.8-5.5-4.5S9 13 12 13s5.5 1.8 5.5 3.5S15 21 12 21Z',
  file: 'M7 2h7l5 5v13a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V3a1 1 0 0 1 1-1Zm7 0v5h5M9 13h6M9 17h6M9 9h2',
  calendar: 'M7 2v4M17 2v4M3 9h18M4 5h16a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V6a1 1 0 0 1 1-1Zm4 8h.01M12 13h.01M16 13h.01M8 17h.01M12 17h.01',
  shield: 'M12 2 4 5v6c0 5 3.4 8.7 8 11 4.6-2.3 8-6 8-11V5l-8-3Zm-2 9 2 2 4-4',
  download: 'M12 3v12m0 0 4-4m-4 4-4-4M4 17v3a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1v-3',
  route: 'M5 19a2 2 0 1 0 0-4 2 2 0 0 0 0 4Zm14-14a2 2 0 1 0 0-4 2 2 0 0 0 0 4ZM7 17l7-10M12 5h4a3 3 0 0 1 3 3v1a3 3 0 0 1-3 3H8a3 3 0 0 0-3 3v1',
  wallet: 'M3 7a2 2 0 0 1 2-2h13a1 1 0 0 1 1 1v2M3 7v11a2 2 0 0 0 2 2h14a1 1 0 0 0 1-1v-8a1 1 0 0 0-1-1H8m9 5h.01',
  checklist: 'M9 5h11M9 12h11M9 19h11M4 4.5l1.2 1.2L7.5 3.4M4 11.5l1.2 1.2 2.3-2.3M4 18.5l1.2 1.2 2.3-2.3',
  phone: 'M6.6 10.8a15.5 15.5 0 0 0 6.6 6.6l2.2-2.2a1 1 0 0 1 1-.25c1.1.37 2.3.57 3.5.57a1 1 0 0 1 1 1V20a1 1 0 0 1-1 1A17 17 0 0 1 3 4a1 1 0 0 1 1-1h3.5a1 1 0 0 1 1 1c0 1.2.2 2.4.57 3.5a1 1 0 0 1-.25 1Z',
};

interface IconProps {
  name: IconName;
  size?: number;
  className?: string;
  style?: CSSProperties;
}

export function Icon({ name, size = 20, className, style }: IconProps) {
  return (
    <svg
      className={className}
      style={style}
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth={1.8}
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
      focusable="false"
    >
      <path d={paths[name]} />
    </svg>
  );
}
