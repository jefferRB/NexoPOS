// Exportación CSV sencilla, sin dependencias externas.

function escapeCsvValue(value: unknown): string {
  const text = value === null || value === undefined ? '' : String(value);
  if (/[",\n;]/.test(text)) {
    return `"${text.replace(/"/g, '""')}"`;
  }
  return text;
}

export interface CsvColumn<T> {
  header: string;
  value: (row: T) => unknown;
}

/** Construye el contenido CSV (con BOM para que Excel detecte UTF-8) a partir de columnas y filas. */
export function buildCsv<T>(columns: CsvColumn<T>[], rows: T[]): string {
  const header = columns.map((c) => escapeCsvValue(c.header)).join(',');
  const lines = rows.map((row) => columns.map((c) => escapeCsvValue(c.value(row))).join(','));
  return ['﻿' + header, ...lines].join('\r\n');
}

/** Genera un archivo CSV y dispara la descarga en el navegador. */
export function downloadCsv<T>(filename: string, columns: CsvColumn<T>[], rows: T[]): void {
  const csv = buildCsv(columns, rows);
  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
  const url = URL.createObjectURL(blob);

  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  URL.revokeObjectURL(url);
}
