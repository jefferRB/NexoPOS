import { useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { StatCard } from '../../components/ui/StatCard';
import { DataState } from '../../components/ui/DataState';
import { EmptyState } from '../../components/ui/EmptyState';
import { DemoModal } from '../../components/ui/DemoModal';
import type { DemoAction } from '../../components/ui/DemoModal';
import { Icon } from '../../components/ui/Icon';
import { BranchCard } from '../../components/dashboard/BranchCard';
import { PriorityAlerts } from '../../components/dashboard/PriorityAlerts';
import { ActivityFeed } from '../../components/dashboard/ActivityFeed';
import { useApiData } from '../../hooks/useApiData';
import { fetchOverview } from '../../api/demoApi';
import { formatCRC, formatNumber } from '../../utils/format';

const demoActions: DemoAction[] = [
  {
    title: 'Registrar venta',
    icon: 'sale',
    description:
      'El punto de venta permitirá registrar ventas por producto completo o por fracción, aplicar descuentos y facturar de inmediato. Aún no está disponible en esta maqueta.',
    points: ['Selección de productos y servicios', 'Venta por fracción (ml, kg)', 'Emisión del comprobante electrónico'],
  },
  {
    title: 'Transferir inventario',
    icon: 'transfer',
    description:
      'Las transferencias moverán existencias entre sucursales y botiquines móviles con trazabilidad completa. Aún no está disponible en esta maqueta.',
    points: ['Origen y destino (sucursal o botiquín)', 'Productos y cantidades', 'Confirmación de recepción'],
  },
  {
    title: 'Crear orden de compra',
    icon: 'plus',
    description:
      'Permitirá generar una orden de compra a partir de la reposición sugerida y enviarla al proveedor correspondiente. Aún no está disponible en esta maqueta.',
    points: ['Basada en el reporte de Reposición', 'Agrupada por proveedor', 'Seguimiento del estado de la orden'],
  },
  {
    title: 'Emitir comprobante',
    icon: 'invoice',
    description:
      'La emisión de facturas y tiquetes electrónicos se configurará con la información fiscal oficial de la empresa. Aún no está disponible en esta maqueta.',
    points: ['Factura electrónica o tiquete', 'Métodos de pago combinados', 'Envío al cliente'],
  },
];

export function DashboardPage() {
  const { data, loading, error, reload } = useApiData(fetchOverview);
  const [activeAction, setActiveAction] = useState<DemoAction | null>(null);

  return (
    <AppShell title="Resumen general">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Resumen general</h2>
          <p className="nx-page-header__subtitle">Operación consolidada de las 3 veterinarias</p>
        </div>
        <div className="nx-page-header__actions">
          {demoActions.map((action) => (
            <button
              key={action.title}
              type="button"
              className={`nx-btn ${action.title === 'Registrar venta' ? 'nx-btn--primary' : 'nx-btn--subtle'}`}
              onClick={() => setActiveAction(action)}
            >
              <span className="nx-btn__icon">
                <Icon name={action.icon} size={17} />
              </span>
              {action.title}
            </button>
          ))}
        </div>
      </div>

      <DataState loading={loading} error={error} onRetry={reload}>
        {data && (
          <>
            <section className="nx-stat-grid" aria-label="Indicadores generales">
              <StatCard
                label="Ventas de hoy"
                value={formatCRC(data.indicators.salesToday)}
                icon="cash"
                tone="ok"
                caption="Total de las 3 veterinarias"
              />
              <StatCard
                label="Tickets emitidos hoy"
                value={formatNumber(data.indicators.ticketsToday)}
                icon="invoice"
                tone="primary"
                caption="Facturas y tiquetes"
              />
              <StatCard
                label="Productos con stock bajo"
                value={formatNumber(data.indicators.lowStockProducts)}
                icon="alert"
                tone="warn"
                caption="Por sucursal, sin mezclar unidades"
              />
              <StatCard
                label="Reposición sugerida"
                value={formatNumber(data.indicators.reorderSuggestedCount)}
                icon="reorder"
                tone="info"
                caption="Según consumo promedio semanal"
              />
              <StatCard
                label="Cuentas por cobrar"
                value={formatCRC(data.indicators.receivablesTotal)}
                icon="receivable"
                tone="info"
                caption="Saldo consolidado"
              />
              <StatCard
                label="Cuentas por pagar"
                value={formatCRC(data.indicators.payablesTotal)}
                icon="payable"
                tone="warn"
                caption="Saldo consolidado"
              />
            </section>

            <section aria-label="Sucursales">
              <div className="nx-branch-grid">
                {data.branches.map((branch) => (
                  <BranchCard key={branch.id} branch={branch} />
                ))}
              </div>
            </section>

            <section className="nx-section nx-grid-2">
              <div className="nx-card">
                <div className="nx-card__header">
                  <div>
                    <div className="nx-card__title">Alertas prioritarias</div>
                    <div className="nx-card__subtitle">Existencias, botiquines, transferencias y reposición</div>
                  </div>
                  <span className="nx-badge nx-badge--low">{data.priorityAlerts.length} alertas</span>
                </div>
                <div className="nx-card__body nx-card__body--flush">
                  {data.priorityAlerts.length > 0 ? (
                    <PriorityAlerts alerts={data.priorityAlerts} />
                  ) : (
                    <EmptyState message="Sin alertas prioritarias por el momento." icon="checklist" />
                  )}
                </div>
              </div>

              <div className="nx-card">
                <div className="nx-card__header">
                  <div className="nx-card__title">Actividad reciente</div>
                </div>
                <div className="nx-card__body nx-card__body--flush">
                  <ActivityFeed items={data.recentActivity} />
                </div>
              </div>
            </section>
          </>
        )}
      </DataState>

      <DemoModal action={activeAction} onClose={() => setActiveAction(null)} />
    </AppShell>
  );
}
