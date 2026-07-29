import { useCallback, useEffect, useRef } from 'react';
import { Icon } from '../ui/Icon';
import { DataState } from '../ui/DataState';
import { useApiData } from '../../hooks/useApiData';
import { fetchInvoice } from '../../api/demoApi';
import { formatCRC, formatDateTime, estimateIncludedTax, formatQuantity } from '../../utils/format';
import { invoiceStatusLabels, invoiceTypeLabels, paymentMethodLabels } from '../../utils/labels';

interface InvoiceDetailModalProps {
  invoiceId: string | null;
  onClose: () => void;
}

export function InvoiceDetailModal({ invoiceId, onClose }: InvoiceDetailModalProps) {
  useEffect(() => {
    if (!invoiceId) {
      return;
    }
    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        onClose();
      }
    };
    document.addEventListener('keydown', onKeyDown);
    return () => document.removeEventListener('keydown', onKeyDown);
  }, [invoiceId, onClose]);

  if (!invoiceId) {
    return null;
  }

  return <InvoiceDetailModalContent invoiceId={invoiceId} onClose={onClose} />;
}

function InvoiceDetailModalContent({ invoiceId, onClose }: { invoiceId: string; onClose: () => void }) {
  const closeRef = useRef<HTMLButtonElement>(null);
  const fetcher = useCallback((signal: AbortSignal) => fetchInvoice(invoiceId, signal), [invoiceId]);
  const { data, loading, error, reload } = useApiData(fetcher);

  useEffect(() => {
    closeRef.current?.focus();
  }, []);

  return (
    <div className="nx-modal__backdrop" onClick={onClose}>
      <div
        className="nx-modal nx-modal--wide"
        role="dialog"
        aria-modal="true"
        aria-labelledby="nx-invoice-title"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="nx-modal__header">
          <span className="nx-modal__icon">
            <Icon name="invoice" size={22} />
          </span>
          <div>
            <h2 className="nx-modal__title" id="nx-invoice-title">
              {data?.summary.number ?? 'Comprobante'}
            </h2>
            {data && <span className="nx-tag">{invoiceTypeLabels[data.summary.type]}</span>}
          </div>
        </div>
        <div className="nx-modal__body">
          <DataState loading={loading} error={error} onRetry={reload} loadingLabel="Cargando comprobante…">
            {data && (
              <div className="nx-product-detail">
                <div className="nx-product-detail__grid">
                  <Field label="Cliente" value={data.summary.customerName} />
                  <Field label="Emitido por" value={data.summary.issuedBy} />
                  <Field label="Sucursal" value={data.summary.branchName} />
                  <Field label="Fecha y hora" value={formatDateTime(data.summary.issuedAt)} />
                  <Field label="Método de pago" value={paymentMethodLabels[data.summary.paymentMethod]} />
                  <Field label="Estado fiscal (demo)" value={invoiceStatusLabels[data.summary.status]} />
                </div>

                <div className="nx-product-detail__section">
                  <h3 className="nx-product-detail__heading">Líneas del comprobante</h3>
                  <div className="nx-table-scroll">
                    <table className="nx-table">
                      <thead>
                        <tr>
                          <th scope="col">Producto o servicio</th>
                          <th scope="col" className="nx-table__num">
                            Cantidad
                          </th>
                          <th scope="col" className="nx-table__num">
                            Precio unitario
                          </th>
                          <th scope="col" className="nx-table__num">
                            Total
                          </th>
                        </tr>
                      </thead>
                      <tbody>
                        {data.lines.map((line) => (
                          <tr key={line.description}>
                            <td>{line.description}</td>
                            <td className="nx-table__num">{formatQuantity(line.quantity, line.unit)}</td>
                            <td className="nx-table__num">{formatCRC(line.unitPrice)}</td>
                            <td className="nx-table__num nx-table__strong">{formatCRC(line.lineTotal)}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>

                <div className="nx-product-detail__section">
                  <div className="nx-product-detail__grid">
                    <Field label="IVA estimado (incluido, 13%)" value={formatCRC(estimateIncludedTax(data.summary.total))} />
                    <Field label="Total del comprobante" value={formatCRC(data.summary.total)} />
                  </div>
                  <p className="nx-modal__footnote">
                    Vista demostrativa. La integración y las reglas fiscales definitivas se configurarán con la
                    información oficial de la empresa. Trazabilidad: {data.summary.branchName} · {data.summary.issuedBy} ·{' '}
                    {formatDateTime(data.summary.issuedAt)}.
                  </p>
                </div>
              </div>
            )}
          </DataState>
        </div>
        <div className="nx-modal__footer">
          <button ref={closeRef} type="button" className="nx-btn nx-btn--primary" onClick={onClose}>
            Cerrar
          </button>
        </div>
      </div>
    </div>
  );
}

function Field({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <div className="nx-metric__label">{label}</div>
      <div className="nx-metric__value">{value}</div>
    </div>
  );
}
