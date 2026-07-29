import { useCallback, useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { StatCard } from '../../components/ui/StatCard';
import { DataState } from '../../components/ui/DataState';
import { EmptyState } from '../../components/ui/EmptyState';
import { Icon } from '../../components/ui/Icon';
import { QuantityDisplay } from '../../components/ui/QuantityDisplay';
import { SupplierOrderModal } from '../../components/reorder/SupplierOrderModal';
import { useApiData } from '../../hooks/useApiData';
import { fetchReorderSuggestions } from '../../api/demoApi';
import { formatCRC, formatNumber } from '../../utils/format';
import { downloadCsv } from '../../utils/csv';
import { reorderPriorityLabels } from '../../utils/labels';
import type { ReorderBasisCode, ReorderSuggestion, SupplierOrder } from '../../types/api';

export function ReorderPage() {
  const [basis, setBasis] = useState<ReorderBasisCode>('weekly');
  const fetcher = useCallback((signal: AbortSignal) => fetchReorderSuggestions(basis, signal), [basis]);
  const { data, loading, error, reload } = useApiData(fetcher);
  const [selectedOrder, setSelectedOrder] = useState<SupplierOrder | null>(null);

  const allItems = data?.supplierOrders.flatMap((order) => order.items.map((item) => ({ ...item, supplierName: order.supplierName }))) ?? [];

  const exportCsv = () => {
    if (!data) return;
    downloadCsv(
      'reposicion-nexopos.csv',
      [
        { header: 'Producto', value: (i: ReorderSuggestion) => i.productName },
        { header: 'Proveedor', value: (i: ReorderSuggestion) => i.supplierName },
        { header: 'Existencia actual', value: (i: ReorderSuggestion) => i.currentStock },
        { header: 'Venta semanal promedio', value: (i: ReorderSuggestion) => i.weeklyAverageSales },
        { header: 'Venta mensual promedio', value: (i: ReorderSuggestion) => i.monthlyAverageSales },
        { header: 'Cobertura estimada (días)', value: (i: ReorderSuggestion) => i.coverageDays },
        { header: 'Cantidad sugerida', value: (i: ReorderSuggestion) => i.suggestedQuantity },
        { header: 'Prioridad', value: (i: ReorderSuggestion) => reorderPriorityLabels[i.priority] },
      ],
      allItems,
    );
  };

  return (
    <AppShell title="Reposición">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Reposición sugerida</h2>
          <p className="nx-page-header__subtitle">Basada en el consumo promedio y las existencias actuales</p>
        </div>
        <div className="nx-page-header__actions">
          <div className="nx-field">
            <label className="nx-field__label" htmlFor="basis-select">
              Base de cálculo
            </label>
            <select
              id="basis-select"
              className="nx-select"
              value={basis}
              onChange={(event) => setBasis(event.target.value as ReorderBasisCode)}
            >
              <option value="weekly">Promedio semanal</option>
              <option value="monthly">Promedio mensual</option>
            </select>
          </div>
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
            <section className="nx-stat-grid" aria-label="Indicadores de reposición">
              <StatCard label="Productos por reponer" value={formatNumber(data.indicators.productsToReorder)} icon="reorder" tone="primary" />
              <StatCard label="Proveedores involucrados" value={formatNumber(data.indicators.suppliersInvolved)} icon="supplier" tone="info" />
              <StatCard label="Valor estimado" value={formatCRC(data.indicators.estimatedValue)} icon="cash" tone="warn" />
              <StatCard label="Días promedio de cobertura" value={`${data.indicators.averageCoverageDays} días`} icon="calendar" tone="ok" />
            </section>

            {data.supplierOrders.length === 0 ? (
              <EmptyState message="No hay productos que requieran reposición con la base de cálculo seleccionada." icon="checklist" />
            ) : (
              data.supplierOrders.map((order) => (
                <div className="nx-supplier-group" key={order.supplierId}>
                  <div className="nx-supplier-group__header">
                    <div>
                      <div className="nx-supplier-group__name">{order.supplierName}</div>
                      <div className="nx-supplier-group__value">{formatCRC(order.estimatedValue)} estimado · {order.items.length} productos</div>
                    </div>
                    <button type="button" className="nx-btn nx-btn--subtle" onClick={() => setSelectedOrder(order)}>
                      Revisar orden
                    </button>
                  </div>
                  <div className="nx-table-scroll">
                    <table className="nx-table">
                      <thead>
                        <tr>
                          <th scope="col">Producto</th>
                          <th scope="col" className="nx-table__num">
                            Existencia actual
                          </th>
                          <th scope="col" className="nx-table__num nx-table__col--hide-mobile">
                            Venta semanal
                          </th>
                          <th scope="col" className="nx-table__num nx-table__col--hide-mobile">
                            Venta mensual
                          </th>
                          <th scope="col" className="nx-table__num">
                            Cobertura
                          </th>
                          <th scope="col" className="nx-table__num">
                            Sugerido
                          </th>
                          <th scope="col">Prioridad</th>
                        </tr>
                      </thead>
                      <tbody>
                        {order.items.map((item) => (
                          <tr key={item.productId}>
                            <td>
                              <div className="nx-table__product">
                                <span className="nx-table__product-name">{item.productName}</span>
                                <span className="nx-table__product-cat">{item.productCode}</span>
                              </div>
                            </td>
                            <td className="nx-table__num">
                              <QuantityDisplay value={item.currentStock} unit={item.unit} />
                            </td>
                            <td className="nx-table__num nx-table__col--hide-mobile">
                              <QuantityDisplay value={item.weeklyAverageSales} unit={item.unit} />
                            </td>
                            <td className="nx-table__num nx-table__col--hide-mobile">
                              <QuantityDisplay value={item.monthlyAverageSales} unit={item.unit} />
                            </td>
                            <td className="nx-table__num">{item.coverageDays} días</td>
                            <td className="nx-table__num nx-table__strong">
                              <QuantityDisplay value={item.suggestedQuantity} unit={item.unit} />
                            </td>
                            <td>
                              <span className={`nx-badge nx-badge--priority-${item.priority}`}>
                                <span className="nx-dot" aria-hidden="true" />
                                {reorderPriorityLabels[item.priority]}
                              </span>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              ))
            )}
          </>
        )}
      </DataState>

      <SupplierOrderModal order={selectedOrder} onClose={() => setSelectedOrder(null)} />
    </AppShell>
  );
}
