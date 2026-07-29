import { useCallback } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { DataState } from '../../components/ui/DataState';
import { StatCard } from '../../components/ui/StatCard';
import { StatusBadge, OperationalBadge } from '../../components/ui/StatusBadge';
import { QuantityDisplay } from '../../components/ui/QuantityDisplay';
import { ActivityFeed } from '../../components/dashboard/ActivityFeed';
import { EmptyState } from '../../components/ui/EmptyState';
import { Icon } from '../../components/ui/Icon';
import { Link } from '../../router/router';
import { useApiData } from '../../hooks/useApiData';
import { fetchBranchDetail } from '../../api/demoApi';
import { formatCRC, formatNumber, formatShortDate } from '../../utils/format';
import { productTypeLabels } from '../../utils/labels';
import type { DailyPerformance } from '../../types/api';

export function BranchDetailPage({ branchId }: { branchId: string }) {
  const fetcher = useCallback(
    (signal: AbortSignal) => fetchBranchDetail(branchId, signal),
    [branchId],
  );
  const { data, loading, error, reload } = useApiData(fetcher);

  return (
    <AppShell title={data ? data.branch.name : 'Sucursal'}>
      <div className="nx-detail-header">
        <Link to="/sucursales" className="nx-btn nx-btn--ghost nx-detail-header__back">
          <span className="nx-btn__icon">
            <Icon name="arrow-left" size={16} />
          </span>
          Volver a sucursales
        </Link>

        {data && (
          <>
            <div className="nx-detail-header__title-row">
              <h2>{data.branch.name}</h2>
              <OperationalBadge isOperational={data.branch.isOperational} />
            </div>
            <div className="nx-detail-header__meta">
              <span className="nx-detail-header__meta-item">
                <Icon name="pin" size={14} /> {data.branch.location}
              </span>
              <span className="nx-detail-header__meta-item">
                <Icon name="phone" size={14} /> {data.branch.phone}
              </span>
              <span className="nx-detail-header__meta-item">
                <Icon name="calendar" size={14} /> {data.branch.schedule}
              </span>
            </div>
          </>
        )}
      </div>

      <DataState loading={loading} error={error} onRetry={reload} loadingLabel="Cargando la sucursal…">
        {data && (
          <>
            <section className="nx-stat-grid" aria-label="Indicadores de la sucursal">
              <StatCard label="Ventas de hoy" value={formatCRC(data.branch.salesToday)} icon="cash" tone="ok" />
              <StatCard label="Tickets emitidos hoy" value={formatNumber(data.branch.ticketsToday)} icon="invoice" tone="primary" />
              <StatCard label="Alertas de stock" value={formatNumber(data.branch.lowStockCount)} icon="alert" tone="warn" />
              <StatCard label="Veterinarios activos" value={formatNumber(data.branch.activeCollaborators)} icon="users" tone="info" />
              <StatCard label="Botiquines asignados" value={formatNumber(data.branch.mobileKitsCount)} icon="kit" tone="primary" />
              <StatCard label="Cuentas por cobrar" value={formatCRC(data.branch.receivablesBalance)} icon="receivable" tone="info" />
              <StatCard label="Cuentas por pagar" value={formatCRC(data.branch.payablesBalance)} icon="payable" tone="warn" />
            </section>

            <section className="nx-card">
              <div className="nx-card__header">
                <div>
                  <div className="nx-card__title">Rendimiento de los últimos 7 días</div>
                  <div className="nx-card__subtitle">Tickets y ventas diarias</div>
                </div>
              </div>
              <div className="nx-card__body">
                <PerformanceBars series={data.weeklyPerformance} />
              </div>
            </section>

            <section className="nx-grid-2 nx-section">
              <div className="nx-card">
                <div className="nx-card__header">
                  <div>
                    <div className="nx-card__title">Inventario de la sucursal</div>
                    <div className="nx-card__subtitle">{data.inventory.length} productos</div>
                  </div>
                </div>
                <div className="nx-card__body nx-card__body--flush">
                  <div className="nx-table-scroll">
                    <table className="nx-table">
                      <thead>
                        <tr>
                          <th scope="col">Producto</th>
                          <th scope="col">Código</th>
                          <th scope="col">Tipo</th>
                          <th scope="col" className="nx-table__num">
                            Cantidad
                          </th>
                          <th scope="col">Estado</th>
                        </tr>
                      </thead>
                      <tbody>
                        {data.inventory.map((item) => (
                          <tr key={item.productId}>
                            <td className="nx-table__product-name">{item.name}</td>
                            <td className="nx-code">{item.internalCode}</td>
                            <td className="nx-muted">{productTypeLabels[item.type]}</td>
                            <td className="nx-table__num nx-table__strong">
                              <QuantityDisplay value={item.quantity} unit={item.unit} />
                            </td>
                            <td>
                              <StatusBadge status={item.status} />
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
                <div className="nx-card">
                  <div className="nx-card__header">
                    <div className="nx-card__title">Productos más vendidos (semana)</div>
                  </div>
                  <div className="nx-card__body">
                    {data.topProducts.length > 0 ? (
                      <div className="nx-top-products">
                        {data.topProducts.map((product, index) => (
                          <div className="nx-top-products__row" key={product.productName}>
                            <span className="nx-top-products__rank">{index + 1}</span>
                            <span className="nx-top-products__name">{product.productName}</span>
                            <QuantityDisplay value={product.quantitySold} unit={product.unit} />
                          </div>
                        ))}
                      </div>
                    ) : (
                      <EmptyState message="Sin datos de productos más vendidos." />
                    )}
                  </div>
                </div>

                <div className="nx-card">
                  <div className="nx-card__header">
                    <div className="nx-card__title">Últimos movimientos</div>
                  </div>
                  <div className="nx-card__body nx-card__body--flush">
                    {data.recentActivity.length > 0 ? (
                      <ActivityFeed items={data.recentActivity} showLocation={false} />
                    ) : (
                      <EmptyState message="Sin movimientos recientes en esta sucursal." />
                    )}
                  </div>
                </div>
              </div>
            </section>
          </>
        )}
      </DataState>
    </AppShell>
  );
}

function PerformanceBars({ series }: { series: DailyPerformance[] }) {
  if (series.length === 0) {
    return <EmptyState message="Sin datos de desempeño reciente." />;
  }

  const maxSales = Math.max(...series.map((d) => d.sales));

  return (
    <div className="nx-bars">
      {series.map((day, index) => {
        const isToday = index === series.length - 1;
        const heightPct = maxSales > 0 ? Math.max(6, Math.round((day.sales / maxSales) * 100)) : 6;
        return (
          <div className="nx-bars__col" key={day.date}>
            <span className="nx-bars__value">{day.tickets} tiq.</span>
            <div className="nx-bars__track">
              <div
                className={`nx-bars__bar${isToday ? ' nx-bars__bar--today' : ''}`}
                style={{ height: `${heightPct}%` }}
                title={`${formatCRC(day.sales)} · ${day.tickets} tickets`}
              />
            </div>
            <span className="nx-bars__label">{isToday ? 'Hoy' : formatShortDate(day.date)}</span>
          </div>
        );
      })}
    </div>
  );
}
