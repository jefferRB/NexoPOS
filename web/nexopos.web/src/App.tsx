import { AppShell } from './components/layout/AppShell';
import { Icon } from './components/ui/Icon';
import { Link, useLocation, useScrollToTopOnNavigate } from './router/router';
import { DashboardPage } from './features/dashboard/DashboardPage';
import { InventoryPage } from './features/inventory/InventoryPage';
import { BranchesPage } from './features/branches/BranchesPage';
import { BranchDetailPage } from './features/branches/BranchDetailPage';
import { ServicesPage } from './features/services/ServicesPage';
import { MobileKitsPage } from './features/mobilekits/MobileKitsPage';
import { InvoicingPage } from './features/invoicing/InvoicingPage';
import { ReorderPage } from './features/reorder/ReorderPage';

function NotFound() {
  return (
    <AppShell title="Página no encontrada">
      <div className="nx-state" style={{ padding: '80px 24px' }}>
        <span className="nx-state__icon">
          <Icon name="inbox" size={26} />
        </span>
        <p className="nx-state__title">Página no encontrada</p>
        <p className="nx-state__text">La ruta solicitada no existe en esta maqueta.</p>
        <Link to="/" className="nx-btn nx-btn--primary">
          Ir al resumen
        </Link>
      </div>
    </AppShell>
  );
}

export default function App() {
  const path = useLocation();
  useScrollToTopOnNavigate(path);

  if (path === '/') {
    return <DashboardPage />;
  }
  if (path === '/inventario') {
    return <InventoryPage />;
  }
  if (path === '/sucursales') {
    return <BranchesPage />;
  }
  if (path === '/servicios') {
    return <ServicesPage />;
  }
  if (path === '/botiquines') {
    return <MobileKitsPage />;
  }
  if (path === '/facturacion') {
    return <InvoicingPage />;
  }
  if (path === '/reposicion') {
    return <ReorderPage />;
  }

  const branchMatch = path.match(/^\/sucursales\/([^/]+)\/?$/);
  if (branchMatch) {
    return <BranchDetailPage branchId={decodeURIComponent(branchMatch[1])} />;
  }

  return <NotFound />;
}
