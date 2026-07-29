import { useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { StatCard } from '../../components/ui/StatCard';
import { DataState } from '../../components/ui/DataState';
import { Icon } from '../../components/ui/Icon';
import { InvoiceDetailModal } from '../../components/invoicing/InvoiceDetailModal';
import { useApiData } from '../../hooks/useApiData';
import { fetchInvoices } from '../../api/demoApi';
import { formatCRC, formatDateTime, formatNumber } from '../../utils/format';
import { downloadCsv } from '../../utils/csv';
import { invoiceStatusLabels, invoiceTypeLabels, paymentMethodLabels } from '../../utils/labels';
import type { InvoiceSummary } from '../../types/api';

export function InvoicingPage() {
  const { data, loading, error, reload } = useApiData(fetchInvoices);
  const [selectedInvoiceId, setSelectedInvoiceId] = useState<string | null>(null);

  const exportCsv = () => {
    if (!data) return;
    downloadCsv(
      'facturacion-nexopos.csv',
      [
        { header: 'Comprobante', value: (i: InvoiceSummary) => i.number },
        { header: 'Fecha y hora', value: (i: InvoiceSummary) => formatDateTime(i.issuedAt) },
        { header: 'Sucursal', value: (i: InvoiceSummary) => i.branchName },
        { header: 'Cliente', value: (i: InvoiceSummary) => i.customerName },
        { header: 'Emitido por', value: (i: InvoiceSummary) => i.issuedBy },
        { header: 'Tipo', value: (i: InvoiceSummary) => invoiceTypeLabels[i.type] },
        { header: 'Método de pago', value: (i: InvoiceSummary) => paymentMethodLabels[i.paymentMethod] },
        { header: 'Total', value: (i: InvoiceSummary) => i.total },
        { header: 'Estado', value: (i: InvoiceSummary) => invoiceStatusLabels[i.status] },
      ],
      data.invoices,
    );
  };

  return (
    <AppShell title="Facturación">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Facturación</h2>
          <p className="nx-page-header__subtitle">Vista de solo lectura de los comprobantes recientes</p>
        </div>
        <div className="nx-page-header__actions">
          <button type="button" className="nx-btn nx-btn--subtle" onClick={exportCsv} disabled={!data}>
            <span className="nx-btn__icon">
              <Icon name="download" size={16} />
            </span>
            Exportar CSV
          </button>
        </div>
      </div>

      <DataState loading={loading} error={error} onRetry={reload}>
        {data && (
          <>
            <section className="nx-stat-grid" aria-label="Indicadores de facturación">
              <StatCard label="Comprobantes emitidos" value={formatNumber(data.indicators.issuedToday)} icon="invoice" tone="primary" />
              <StatCard label="Aceptados" value={formatNumber(data.indicators.accepted)} icon="checklist" tone="ok" />
              <StatCard label="Pendientes" value={formatNumber(data.indicators.pending)} icon="alert" tone="warn" />
              <StatCard label="Anulados" value={formatNumber(data.indicators.voided)} icon="alert" tone="warn" />
              <StatCard label="Ticket promedio" value={formatCRC(data.indicators.averageTicket)} icon="cash" tone="info" />
            </section>

            <div className="nx-card">
              <div className="nx-card__body nx-card__body--flush">
                <div className="nx-table-scroll">
                  <table className="nx-table">
                    <thead>
                      <tr>
                        <th scope="col">Comprobante</th>
                        <th scope="col" className="nx-table__col--hide-mobile">
                          Fecha y hora
                        </th>
                        <th scope="col">Sucursal</th>
                        <th scope="col" className="nx-table__col--hide-mobile">
                          Cliente
                        </th>
                        <th scope="col" className="nx-table__col--hide-mobile">
                          Emitido por
                        </th>
                        <th scope="col">Tipo</th>
                        <th scope="col" className="nx-table__col--hide-mobile">
                          Método de pago
                        </th>
                        <th scope="col" className="nx-table__num">
                          Total
                        </th>
                        <th scope="col">Estado</th>
                        <th scope="col">
                          <span className="nx-visually-hidden">Acciones</span>
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      {data.invoices.map((invoice) => (
                        <tr key={invoice.id}>
                          <td className="nx-code">{invoice.number}</td>
                          <td className="nx-table__col--hide-mobile">{formatDateTime(invoice.issuedAt)}</td>
                          <td>{invoice.branchName}</td>
                          <td className="nx-table__col--hide-mobile">{invoice.customerName}</td>
                          <td className="nx-table__col--hide-mobile">{invoice.issuedBy}</td>
                          <td>
                            <span className="nx-tag">{invoiceTypeLabels[invoice.type]}</span>
                          </td>
                          <td className="nx-table__col--hide-mobile">{paymentMethodLabels[invoice.paymentMethod]}</td>
                          <td className="nx-table__num nx-table__strong">{formatCRC(invoice.total)}</td>
                          <td>
                            <span className={`nx-badge nx-badge--${invoice.status}`}>
                              <span className="nx-dot" aria-hidden="true" />
                              {invoiceStatusLabels[invoice.status]}
                            </span>
                          </td>
                          <td>
                            <button type="button" className="nx-btn nx-btn--ghost" onClick={() => setSelectedInvoiceId(invoice.id)}>
                              Ver
                            </button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>

            <p className="nx-muted" style={{ marginTop: 14, fontSize: 13 }}>
              Vista demostrativa. La integración y las reglas fiscales se configurarán con la información oficial de la empresa.
            </p>
          </>
        )}
      </DataState>

      <InvoiceDetailModal invoiceId={selectedInvoiceId} onClose={() => setSelectedInvoiceId(null)} />
    </AppShell>
  );
}
