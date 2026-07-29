import { useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { DataState } from '../../components/ui/DataState';
import { DemoModal } from '../../components/ui/DemoModal';
import { Icon } from '../../components/ui/Icon';
import { BranchCard } from '../../components/dashboard/BranchCard';
import { useApiData } from '../../hooks/useApiData';
import { fetchOverview } from '../../api/demoApi';

export function BranchesPage() {
  const { data, loading, error, reload } = useApiData(fetchOverview);
  const [showAddBranch, setShowAddBranch] = useState(false);

  return (
    <AppShell title="Sucursales">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Sucursales</h2>
          <p className="nx-page-header__subtitle">
            Estado operativo y desempeño de cada veterinaria de Grupo Veterinario Demo
          </p>
        </div>
      </div>

      <DataState loading={loading} error={error} onRetry={reload} isEmpty={data?.branches.length === 0}>
        {data && (
          <div className="nx-branch-grid">
            {data.branches.map((branch) => (
              <BranchCard key={branch.id} branch={branch} />
            ))}

            <button type="button" className="nx-add-branch-card" onClick={() => setShowAddBranch(true)}>
              <span className="nx-add-branch-card__icon">
                <Icon name="plus" size={22} />
              </span>
              <span className="nx-add-branch-card__label">Agregar nueva sucursal</span>
            </button>
          </div>
        )}
      </DataState>

      <DemoModal
        action={
          showAddBranch
            ? {
                title: 'Agregar nueva sucursal',
                icon: 'branch',
                description:
                  'Permitirá dar de alta una nueva veterinaria con su propio inventario, colaboradores y horario. Aún no está disponible en esta maqueta.',
                points: ['Datos generales y horario', 'Inventario inicial', 'Asignación de colaboradores'],
              }
            : null
        }
        onClose={() => setShowAddBranch(false)}
      />
    </AppShell>
  );
}
