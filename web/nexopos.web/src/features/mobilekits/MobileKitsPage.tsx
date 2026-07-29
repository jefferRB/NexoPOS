import { useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { DataState } from '../../components/ui/DataState';
import { Icon } from '../../components/ui/Icon';
import { MobileKitStatusBadge } from '../../components/ui/StatusBadge';
import { DemoModal } from '../../components/ui/DemoModal';
import type { DemoAction } from '../../components/ui/DemoModal';
import { KitContentModal } from '../../components/mobilekits/KitContentModal';
import { useApiData } from '../../hooks/useApiData';
import { fetchMobileKits } from '../../api/demoApi';
import { formatCRC, formatRelative } from '../../utils/format';
import type { MobileKit } from '../../types/api';

const kitActions: DemoAction[] = [
  {
    title: 'Transferir al botiquín',
    icon: 'transfer',
    description: 'Permitirá enviar productos desde una sucursal hacia el botiquín del veterinario. Aún no está disponible en esta maqueta.',
    points: ['Selección de productos y cantidades', 'Confirmación de recepción por el veterinario'],
  },
  {
    title: 'Registrar consumo',
    icon: 'sale',
    description: 'Permitirá descontar del botiquín los productos usados durante una visita a domicilio. Aún no está disponible en esta maqueta.',
    points: ['Selección del servicio o producto aplicado', 'Descuento automático del botiquín'],
  },
  {
    title: 'Conciliar inventario',
    icon: 'checklist',
    description: 'Permitirá comparar el contenido físico del botiquín contra el sistema y justificar diferencias. Aún no está disponible en esta maqueta.',
    points: ['Conteo físico del botiquín', 'Registro de diferencias', 'Aprobación del ajuste'],
  },
  {
    title: 'Devolver existencias',
    icon: 'receive',
    description: 'Permitirá regresar productos no utilizados del botiquín hacia la sucursal de origen. Aún no está disponible en esta maqueta.',
    points: ['Selección de productos a devolver', 'Actualización del inventario de la sucursal'],
  },
];

export function MobileKitsPage() {
  const { data, loading, error, reload } = useApiData(fetchMobileKits);
  const [selectedKit, setSelectedKit] = useState<MobileKit | null>(null);
  const [activeAction, setActiveAction] = useState<DemoAction | null>(null);

  return (
    <AppShell title="Botiquines móviles">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Botiquines móviles</h2>
          <p className="nx-page-header__subtitle">Inventario que cada veterinario lleva en sus visitas a domicilio</p>
        </div>
        <div className="nx-page-header__actions">
          {kitActions.map((action) => (
            <button key={action.title} type="button" className="nx-btn nx-btn--subtle" onClick={() => setActiveAction(action)}>
              <span className="nx-btn__icon">
                <Icon name={action.icon} size={16} />
              </span>
              {action.title}
            </button>
          ))}
        </div>
      </div>

      <DataState loading={loading} error={error} onRetry={reload} isEmpty={data?.length === 0}>
        {data && (
          <div className="nx-kit-grid">
            {data.map((kit) => (
              <article className="nx-kit-card" key={kit.id}>
                <div className="nx-kit-card__head">
                  <div>
                    <h3 className="nx-kit-card__name">{kit.name}</h3>
                    <p className="nx-kit-card__assignee">
                      {kit.assignedTo} · {kit.homeBranchName}
                    </p>
                  </div>
                  <MobileKitStatusBadge status={kit.status} />
                </div>

                <div className="nx-kit-card__stats">
                  <div>
                    <div className="nx-metric__label">Productos</div>
                    <div className="nx-metric__value">{kit.stock.length}</div>
                  </div>
                  <div>
                    <div className="nx-metric__label">Valor estimado</div>
                    <div className="nx-metric__value">{formatCRC(kit.estimatedValue)}</div>
                  </div>
                  <div>
                    <div className="nx-metric__label">Última transferencia</div>
                    <div className="nx-metric__value" style={{ fontSize: 13 }}>
                      {kit.lastTransferAt ? formatRelative(kit.lastTransferAt) : 'Sin registro'}
                    </div>
                  </div>
                  <div>
                    <div className="nx-metric__label">Último consumo</div>
                    <div className="nx-metric__value" style={{ fontSize: 13 }}>
                      {kit.lastConsumptionAt ? formatRelative(kit.lastConsumptionAt) : 'Sin registro'}
                    </div>
                  </div>
                </div>

                {kit.alerts.length > 0 && (
                  <div className="nx-kit-card__alerts">
                    {kit.alerts.map((alert) => (
                      <span key={alert} className="nx-inline">
                        <Icon name="alert" size={13} /> {alert}
                      </span>
                    ))}
                  </div>
                )}

                <div className="nx-kit-card__foot">
                  <button type="button" className="nx-btn nx-btn--subtle" onClick={() => setSelectedKit(kit)}>
                    Ver contenido
                  </button>
                </div>
              </article>
            ))}
          </div>
        )}
      </DataState>

      <KitContentModal kit={selectedKit} onClose={() => setSelectedKit(null)} />
      <DemoModal action={activeAction} onClose={() => setActiveAction(null)} />
    </AppShell>
  );
}
