import { useCallback, useEffect, useRef } from 'react';
import { Icon } from '../ui/Icon';
import { DataState } from '../ui/DataState';
import { StatusBadge } from '../ui/StatusBadge';
import { QuantityDisplay } from '../ui/QuantityDisplay';
import { useApiData } from '../../hooks/useApiData';
import { fetchProductDetail } from '../../api/demoApi';
import { formatDecimal, formatQuantity } from '../../utils/format';
import type { BranchRef, ReorderStatusCode } from '../../types/api';

interface ProductDetailModalProps {
  productId: string | null;
  branches: BranchRef[];
  onClose: () => void;
}

const reorderStatusLabel: Record<ReorderStatusCode, string> = {
  'needs-reorder': 'Requiere reposición',
  sufficient: 'Cobertura suficiente',
  'no-data': 'Sin historial de venta',
};

export function ProductDetailModal({ productId, branches, onClose }: ProductDetailModalProps) {
  useEscapeToClose(!!productId, onClose);

  if (!productId) {
    return null;
  }

  return <ProductDetailModalContent productId={productId} branches={branches} onClose={onClose} />;
}

function useEscapeToClose(active: boolean, onClose: () => void) {
  useEffect(() => {
    if (!active) {
      return;
    }
    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        onClose();
      }
    };
    document.addEventListener('keydown', onKeyDown);
    return () => document.removeEventListener('keydown', onKeyDown);
  }, [active, onClose]);
}

function ProductDetailModalContent({
  productId,
  branches,
  onClose,
}: {
  productId: string;
  branches: BranchRef[];
  onClose: () => void;
}) {
  const closeRef = useRef<HTMLButtonElement>(null);
  const fetcher = useCallback((signal: AbortSignal) => fetchProductDetail(productId, signal), [productId]);
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
        aria-labelledby="nx-product-title"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="nx-modal__header">
          <span className="nx-modal__icon">
            <Icon name="inventory" size={22} />
          </span>
          <div>
            <h2 className="nx-modal__title" id="nx-product-title">
              {data?.summary.name ?? 'Detalle del producto'}
            </h2>
            {data && <span className="nx-tag">{data.summary.internalCode}</span>}
          </div>
        </div>
        <div className="nx-modal__body">
          <DataState loading={loading} error={error} onRetry={reload} loadingLabel="Cargando producto…">
            {data && (
              <div className="nx-product-detail">
                <div className="nx-product-detail__grid">
                  <Field label="Código de fabricante" value={data.summary.manufacturerBarcode} />
                  <Field label="Proveedor" value={data.summary.supplierName} />
                  <Field
                    label="Unidad de compra"
                    value={data.purchaseUnitLabel ?? `Unidad individual (${data.summary.unit})`}
                  />
                  <Field
                    label="Unidad base"
                    value={data.baseUnitsPerPurchaseUnit ? `${formatDecimal(data.baseUnitsPerPurchaseUnit)} ${data.summary.unit} por presentación` : data.summary.unit}
                  />
                </div>

                <div className="nx-product-detail__section">
                  <h3 className="nx-product-detail__heading">Existencias por ubicación</h3>
                  <div className="nx-product-detail__stock-grid">
                    {branches.map((branch) => (
                      <div key={branch.id} className="nx-product-detail__stock-item">
                        <span className="nx-metric__label">{branch.name}</span>
                        <QuantityDisplay
                          className="nx-metric__value"
                          value={data.summary.stockByBranch[branch.id] ?? 0}
                          unit={data.summary.unit}
                        />
                      </div>
                    ))}
                    <div className="nx-product-detail__stock-item">
                      <span className="nx-metric__label">Botiquines móviles</span>
                      <QuantityDisplay className="nx-metric__value" value={data.summary.mobileKitsStock} unit={data.summary.unit} />
                    </div>
                    <div className="nx-product-detail__stock-item nx-product-detail__stock-item--total">
                      <span className="nx-metric__label">Total</span>
                      <QuantityDisplay className="nx-metric__value" value={data.summary.total} unit={data.summary.unit} />
                    </div>
                  </div>
                </div>

                <div className="nx-product-detail__section">
                  <h3 className="nx-product-detail__heading">Reposición</h3>
                  <div className="nx-product-detail__grid">
                    <Field label="Consumo promedio semanal" value={formatQuantity(data.weeklyAverageSales, data.summary.unit)} />
                    <Field label="Consumo promedio mensual" value={formatQuantity(data.monthlyAverageSales, data.summary.unit)} />
                    <Field
                      label="Días de cobertura estimados"
                      value={data.coverageDays != null ? `${formatDecimal(data.coverageDays)} días` : 'Sin datos suficientes'}
                    />
                    <Field label="Estado de reposición" value={reorderStatusLabel[data.reorderStatus]} />
                  </div>
                </div>

                <div className="nx-product-detail__footer">
                  <StatusBadge status={data.summary.status} />
                  <span className="nx-muted">Estado consolidado del producto en todas las ubicaciones.</span>
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
