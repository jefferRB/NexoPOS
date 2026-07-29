import type { ReactNode } from 'react';
import { EmptyState } from './EmptyState';

export interface ResponsiveTableColumn<T> {
  key: string;
  header: string;
  render: (row: T) => ReactNode;
  align?: 'left' | 'right';
  /** Oculta la columna en anchos angostos (celular) para evitar tablas demasiado densas. */
  hideOnMobile?: boolean;
}

interface ResponsiveTableProps<T> {
  columns: ResponsiveTableColumn<T>[];
  rows: T[];
  getRowKey: (row: T) => string;
  emptyMessage?: string;
}

/**
 * Tabla con scroll horizontal contenido, columnas alineadas y una estrategia
 * responsive simple (ocultar columnas secundarias en pantallas angostas) en
 * lugar de reescribir la tabla como tarjetas.
 */
export function ResponsiveTable<T>({ columns, rows, getRowKey, emptyMessage }: ResponsiveTableProps<T>) {
  if (rows.length === 0) {
    return <EmptyState message={emptyMessage ?? 'No hay registros para mostrar.'} />;
  }

  return (
    <div className="nx-table-scroll">
      <table className="nx-table">
        <thead>
          <tr>
            {columns.map((column) => (
              <th
                key={column.key}
                scope="col"
                className={[
                  column.align === 'right' ? 'nx-table__num' : '',
                  column.hideOnMobile ? 'nx-table__col--hide-mobile' : '',
                ]
                  .filter(Boolean)
                  .join(' ')}
              >
                {column.header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.map((row) => (
            <tr key={getRowKey(row)}>
              {columns.map((column) => (
                <td
                  key={column.key}
                  className={[
                    column.align === 'right' ? 'nx-table__num' : '',
                    column.hideOnMobile ? 'nx-table__col--hide-mobile' : '',
                  ]
                    .filter(Boolean)
                    .join(' ')}
                >
                  {column.render(row)}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
