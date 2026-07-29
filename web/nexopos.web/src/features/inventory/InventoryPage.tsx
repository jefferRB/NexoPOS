import { useMemo, useState } from 'react';
import { AppShell } from '../../components/layout/AppShell';
import { DataState } from '../../components/ui/DataState';
import { StatusBadge } from '../../components/ui/StatusBadge';
import { QuantityDisplay } from '../../components/ui/QuantityDisplay';
import { Icon } from '../../components/ui/Icon';
import { ProductDetailModal } from '../../components/inventory/ProductDetailModal';
import { useApiData } from '../../hooks/useApiData';
import { fetchInventory } from '../../api/demoApi';
import { evaluateStatus } from '../../utils/inventory';
import { downloadCsv } from '../../utils/csv';
import { productTypeLabels, unitLabels } from '../../utils/labels';
import type { InventoryItem, ProductTypeCode, StockStatusCode, UnitCode } from '../../types/api';

const PAGE_SIZE = 8;
const ALL = 'all';

type StatusFilter = StockStatusCode | typeof ALL;
type UnitFilter = UnitCode | typeof ALL;
type TypeFilter = ProductTypeCode | 'fractionable' | typeof ALL;

export function InventoryPage() {
  const { data, loading, error, reload } = useApiData(fetchInventory);

  const [search, setSearch] = useState('');
  const [locationFilter, setLocationFilter] = useState<string>(ALL);
  const [statusFilter, setStatusFilter] = useState<StatusFilter>(ALL);
  const [typeFilter, setTypeFilter] = useState<TypeFilter>(ALL);
  const [unitFilter, setUnitFilter] = useState<UnitFilter>(ALL);
  const [supplierFilter, setSupplierFilter] = useState<string>(ALL);
  const [page, setPage] = useState(1);
  const [selectedProductId, setSelectedProductId] = useState<string | null>(null);

  const isMobileKitsLocation = locationFilter === 'mobile-kits';

  const statusOf = (item: InventoryItem): StockStatusCode => {
    if (locationFilter === ALL) {
      return item.status;
    }
    if (isMobileKitsLocation) {
      return evaluateStatus(item.mobileKitsStock, item.minimum);
    }
    return evaluateStatus(item.stockByBranch[locationFilter] ?? 0, item.minimum);
  };

  const suppliers = useMemo(() => {
    if (!data) return [];
    const seen = new Map<string, string>();
    data.items.forEach((item) => seen.set(item.supplierId, item.supplierName));
    return Array.from(seen, ([id, name]) => ({ id, name }));
  }, [data]);

  const filtered = useMemo(() => {
    if (!data) {
      return [];
    }
    const term = search.trim().toLowerCase();
    return data.items.filter((item) => {
      const matchesSearch =
        term.length === 0 ||
        item.name.toLowerCase().includes(term) ||
        item.internalCode.toLowerCase().includes(term) ||
        item.manufacturerBarcode.toLowerCase().includes(term) ||
        item.category.toLowerCase().includes(term) ||
        item.supplierName.toLowerCase().includes(term);

      const matchesStatus = statusFilter === ALL || statusOf(item) === statusFilter;
      const matchesType =
        typeFilter === ALL ||
        (typeFilter === 'fractionable' ? item.isFractionable : item.type === typeFilter);
      const matchesUnit = unitFilter === ALL || item.unit === unitFilter;
      const matchesSupplier = supplierFilter === ALL || item.supplierId === supplierFilter;

      return matchesSearch && matchesStatus && matchesType && matchesUnit && matchesSupplier;
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data, search, statusFilter, typeFilter, unitFilter, supplierFilter, locationFilter]);

  const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE));
  const currentPage = Math.min(page, totalPages);
  const pageItems = filtered.slice((currentPage - 1) * PAGE_SIZE, currentPage * PAGE_SIZE);

  const resetPage = () => setPage(1);
  const branches = data?.branches ?? [];

  const exportCsv = () => {
    if (!data) return;
    downloadCsv(
      'inventario-nexopos.csv',
      [
        { header: 'Producto', value: (i: InventoryItem) => i.name },
        { header: 'Código interno', value: (i: InventoryItem) => i.internalCode },
        { header: 'Código fabricante', value: (i: InventoryItem) => i.manufacturerBarcode },
        { header: 'Tipo', value: (i: InventoryItem) => productTypeLabels[i.type] },
        { header: 'Unidad base', value: (i: InventoryItem) => i.unit },
        ...branches.map((branch) => ({
          header: branch.name,
          value: (i: InventoryItem) => i.stockByBranch[branch.id] ?? 0,
        })),
        { header: 'Botiquines', value: (i: InventoryItem) => i.mobileKitsStock },
        { header: 'Total', value: (i: InventoryItem) => i.total },
        { header: 'Estado', value: (i: InventoryItem) => i.status },
      ],
      filtered,
    );
  };

  return (
    <AppShell title="Inventario">
      <div className="nx-page-header">
        <div>
          <h2 className="nx-page-header__title">Inventario multi-sucursal</h2>
          <p className="nx-page-header__subtitle">Existencias por sucursal y botiquines móviles, en su unidad base</p>
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
            <div className="nx-toolbar">
              <div className="nx-search">
                <span className="nx-search__icon">
                  <Icon name="search" size={18} />
                </span>
                <input
                  type="search"
                  className="nx-input"
                  placeholder="Buscar por nombre, código, categoría o proveedor"
                  aria-label="Buscar producto"
                  value={search}
                  onChange={(event) => {
                    setSearch(event.target.value);
                    resetPage();
                  }}
                />
              </div>

              <div className="nx-field">
                <label className="nx-field__label" htmlFor="location-filter">
                  Ubicación
                </label>
                <select
                  id="location-filter"
                  className="nx-select"
                  value={locationFilter}
                  onChange={(event) => {
                    setLocationFilter(event.target.value);
                    resetPage();
                  }}
                >
                  <option value={ALL}>Todas las ubicaciones</option>
                  {branches.map((branch) => (
                    <option key={branch.id} value={branch.id}>
                      {branch.name}
                    </option>
                  ))}
                  <option value="mobile-kits">Botiquines móviles</option>
                </select>
              </div>

              <div className="nx-field">
                <label className="nx-field__label" htmlFor="type-filter">
                  Tipo
                </label>
                <select
                  id="type-filter"
                  className="nx-select"
                  value={typeFilter}
                  onChange={(event) => {
                    setTypeFilter(event.target.value as TypeFilter);
                    resetPage();
                  }}
                >
                  <option value={ALL}>Todos los tipos</option>
                  <option value="fractionable">Fraccionable</option>
                  <option value="medication">Medicamento</option>
                  <option value="food">Alimento</option>
                  <option value="clinical-supply">Insumo clínico</option>
                </select>
              </div>

              <div className="nx-field">
                <label className="nx-field__label" htmlFor="unit-filter">
                  Unidad
                </label>
                <select
                  id="unit-filter"
                  className="nx-select"
                  value={unitFilter}
                  onChange={(event) => {
                    setUnitFilter(event.target.value as UnitFilter);
                    resetPage();
                  }}
                >
                  <option value={ALL}>Todas las unidades</option>
                  <option value="unit">{unitLabels.unit}</option>
                  <option value="ml">{unitLabels.ml}</option>
                  <option value="kg">{unitLabels.kg}</option>
                </select>
              </div>

              <div className="nx-field">
                <label className="nx-field__label" htmlFor="supplier-filter">
                  Proveedor
                </label>
                <select
                  id="supplier-filter"
                  className="nx-select"
                  value={supplierFilter}
                  onChange={(event) => {
                    setSupplierFilter(event.target.value);
                    resetPage();
                  }}
                >
                  <option value={ALL}>Todos los proveedores</option>
                  {suppliers.map((supplier) => (
                    <option key={supplier.id} value={supplier.id}>
                      {supplier.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="nx-field">
                <label className="nx-field__label" htmlFor="status-filter">
                  Estado
                </label>
                <select
                  id="status-filter"
                  className="nx-select"
                  value={statusFilter}
                  onChange={(event) => {
                    setStatusFilter(event.target.value as StatusFilter);
                    resetPage();
                  }}
                >
                  <option value={ALL}>Todos los estados</option>
                  <option value="available">Disponible</option>
                  <option value="low">Bajo</option>
                  <option value="out">Agotado</option>
                </select>
              </div>
            </div>

            <div className="nx-card">
              <div className="nx-card__body nx-card__body--flush">
                <div className="nx-table-scroll">
                  <table className="nx-table">
                    <thead>
                      <tr>
                        <th scope="col">Producto</th>
                        <th scope="col">Código interno</th>
                        <th scope="col" className="nx-table__col--hide-mobile">
                          Código fabricante
                        </th>
                        <th scope="col">Tipo</th>
                        <th scope="col" className="nx-table__col--hide-mobile">
                          Unidad base
                        </th>
                        {branches.map((branch) => (
                          <th key={branch.id} scope="col" className="nx-table__num nx-table__col--hide-mobile">
                            {branch.name.replace('Veterinaria ', '')}
                          </th>
                        ))}
                        <th scope="col" className="nx-table__num nx-table__col--hide-mobile">
                          Botiquines
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
                      {pageItems.map((item) => (
                        <tr key={item.productId}>
                          <td>
                            <div className="nx-table__product">
                              <span className="nx-table__product-name">{item.name}</span>
                              <span className="nx-table__product-cat">{item.category}</span>
                            </div>
                          </td>
                          <td className="nx-code">{item.internalCode}</td>
                          <td className="nx-code nx-table__col--hide-mobile">{item.manufacturerBarcode}</td>
                          <td>
                            <span className="nx-tag">{productTypeLabels[item.type]}</span>
                          </td>
                          <td className="nx-table__col--hide-mobile">{unitLabels[item.unit]}</td>
                          {branches.map((branch) => (
                            <td
                              key={branch.id}
                              className="nx-table__num nx-table__col--hide-mobile"
                              style={locationFilter === branch.id ? { color: 'var(--nx-text)', fontWeight: 600 } : undefined}
                            >
                              <QuantityDisplay value={item.stockByBranch[branch.id] ?? 0} unit={item.unit} />
                            </td>
                          ))}
                          <td
                            className="nx-table__num nx-table__col--hide-mobile"
                            style={isMobileKitsLocation ? { color: 'var(--nx-text)', fontWeight: 600 } : undefined}
                          >
                            <QuantityDisplay value={item.mobileKitsStock} unit={item.unit} />
                          </td>
                          <td className="nx-table__num nx-table__strong">
                            <QuantityDisplay value={item.total} unit={item.unit} />
                          </td>
                          <td>
                            <StatusBadge status={statusOf(item)} />
                          </td>
                          <td>
                            <button
                              type="button"
                              className="nx-btn nx-btn--ghost"
                              onClick={() => setSelectedProductId(item.productId)}
                            >
                              Ver detalle
                            </button>
                          </td>
                        </tr>
                      ))}

                      {pageItems.length === 0 && (
                        <tr>
                          <td colSpan={branches.length + 9}>
                            <div className="nx-state" style={{ padding: '40px 20px' }}>
                              <span className="nx-state__icon">
                                <Icon name="search" size={22} />
                              </span>
                              <p className="nx-state__text">Ningún producto coincide con los filtros aplicados.</p>
                            </div>
                          </td>
                        </tr>
                      )}
                    </tbody>
                  </table>
                </div>

                {filtered.length > 0 && (
                  <div className="nx-pagination">
                    <span>
                      Mostrando {(currentPage - 1) * PAGE_SIZE + 1}–
                      {Math.min(currentPage * PAGE_SIZE, filtered.length)} de {filtered.length} productos
                    </span>
                    <div className="nx-pagination__controls">
                      <button
                        type="button"
                        className="nx-page-btn"
                        onClick={() => setPage((p) => Math.max(1, p - 1))}
                        disabled={currentPage <= 1}
                        aria-label="Página anterior"
                      >
                        <Icon name="arrow-left" size={16} />
                      </button>
                      <span>
                        Página {currentPage} de {totalPages}
                      </span>
                      <button
                        type="button"
                        className="nx-page-btn"
                        onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
                        disabled={currentPage >= totalPages}
                        aria-label="Página siguiente"
                      >
                        <Icon name="chevron-right" size={16} />
                      </button>
                    </div>
                  </div>
                )}
              </div>
            </div>
          </>
        )}
      </DataState>

      <ProductDetailModal
        productId={selectedProductId}
        branches={data?.branches ?? []}
        onClose={() => setSelectedProductId(null)}
      />
    </AppShell>
  );
}
