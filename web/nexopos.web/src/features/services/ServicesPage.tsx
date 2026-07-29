import { useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { DataState } from '../../components/ui/DataState';
import { Icon } from '../../components/ui/Icon';
import { ServiceCompositionModal } from '../../components/services/ServiceCompositionModal';
import { useApiData } from '../../hooks/useApiData';
import { fetchServices } from '../../api/demoApi';
import { formatCRC } from '../../utils/format';
import type { Service } from '../../types/api';

export function ServicesPage() {
  const { data, loading, error, reload } = useApiData(fetchServices);
  const [selected, setSelected] = useState<Service | null>(null);

  return (
    <AppShell title="Servicios">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Servicios y paquetes</h2>
          <p className="nx-page-header__subtitle">
            Servicios compuestos por productos e insumos de inventario y tiempo de veterinario
          </p>
        </div>
      </div>

      <DataState loading={loading} error={error} onRetry={reload} isEmpty={data?.length === 0}>
        {data && (
          <div className="nx-service-grid">
            {data.map((service) => (
              <article className="nx-service-card" key={service.id}>
                <div className="nx-service-card__head">
                  <div>
                    <h3 className="nx-service-card__name">{service.name}</h3>
                    <p className="nx-service-card__desc">{service.description}</p>
                  </div>
                  <span className="nx-service-card__price">{formatCRC(service.price)}</span>
                </div>
                <div className="nx-service-card__meta">
                  <span className="nx-inline">
                    <Icon name="stethoscope" size={14} /> {service.durationMinutes} min
                  </span>
                  <span className="nx-inline">
                    <Icon name="inventory" size={14} /> {service.components.filter((c) => c.linksToInventory).length} insumos de inventario
                  </span>
                </div>
                <button type="button" className="nx-btn nx-btn--subtle" onClick={() => setSelected(service)}>
                  Ver composición
                  <span className="nx-btn__icon">
                    <Icon name="chevron-right" size={16} />
                  </span>
                </button>
              </article>
            ))}
          </div>
        )}
      </DataState>

      <ServiceCompositionModal service={selected} onClose={() => setSelected(null)} />
    </AppShell>
  );
}
