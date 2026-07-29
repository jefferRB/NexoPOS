import { useEffect, useRef, useState } from 'react';
import { Icon } from '../ui/Icon';
import { DemoModal } from '../ui/DemoModal';
import { QuantityDisplay } from '../ui/QuantityDisplay';
import { formatCRC } from '../../utils/format';
import { reorderPriorityLabels } from '../../utils/labels';
import type { SupplierOrder } from '../../types/api';

interface SupplierOrderModalProps {
  order: SupplierOrder | null;
  onClose: () => void;
}

export function SupplierOrderModal({ order, onClose }: SupplierOrderModalProps) {
  const closeRef = useRef<HTMLButtonElement>(null);
  const [showGenerate, setShowGenerate] = useState(false);

  useEffect(() => {
    if (!order) {
      setShowGenerate(false);
      return;
    }
    closeRef.current?.focus();
    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        onClose();
      }
    };
    document.addEventListener('keydown', onKeyDown);
    return () => document.removeEventListener('keydown', onKeyDown);
  }, [order, onClose]);

  if (!order) {
    return null;
  }

  return (
    <>
      <div className="nx-modal__backdrop" onClick={onClose}>
        <div
          className="nx-modal nx-modal--wide"
          role="dialog"
          aria-modal="true"
          aria-labelledby="nx-order-title"
          onClick={(event) => event.stopPropagation()}
        >
          <div className="nx-modal__header">
            <span className="nx-modal__icon">
              <Icon name="supplier" size={22} />
            </span>
            <div>
              <h2 className="nx-modal__title" id="nx-order-title">
                {order.supplierName}
              </h2>
              <span className="nx-tag">{formatCRC(order.estimatedValue)} estimado</span>
            </div>
          </div>
          <div className="nx-modal__body">
            <div className="nx-table-scroll">
              <table className="nx-table">
                <thead>
                  <tr>
                    <th scope="col">Producto</th>
                    <th scope="col" className="nx-table__num">
                      Existencia
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
                      <td>{item.productName}</td>
                      <td className="nx-table__num">
                        <QuantityDisplay value={item.currentStock} unit={item.unit} />
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
          <div className="nx-modal__footer">
            <button ref={closeRef} type="button" className="nx-btn nx-btn--ghost" onClick={onClose}>
              Cerrar
            </button>
            <button type="button" className="nx-btn nx-btn--primary" onClick={() => setShowGenerate(true)}>
              Generar propuesta de orden
            </button>
          </div>
        </div>
      </div>

      <DemoModal
        action={
          showGenerate
            ? {
                title: 'Generar propuesta de orden',
                icon: 'plus',
                description: `Se generará una propuesta de orden de compra para ${order.supplierName} con las cantidades sugeridas. Aún no está disponible en esta maqueta.`,
                points: ['Cantidades basadas en el consumo promedio', 'Revisión antes de enviar al proveedor', 'Seguimiento del estado de la orden'],
              }
            : null
        }
        onClose={() => setShowGenerate(false)}
      />
    </>
  );
}
