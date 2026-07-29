import { Icon } from '../ui/Icon';
import { OperationalBadge } from '../ui/StatusBadge';
import { Link } from '../../router/router';
import { formatCRC, formatNumber } from '../../utils/format';
import type { BranchSummary } from '../../types/api';

export function BranchCard({ branch }: { branch: BranchSummary }) {
  return (
    <article className="nx-branch-card">
      <div className="nx-branch-card__head">
        <div>
          <h3 className="nx-branch-card__name">{branch.name}</h3>
          <p className="nx-branch-card__loc">
            <Icon name="pin" size={14} />
            {branch.location}
          </p>
        </div>
        <OperationalBadge isOperational={branch.isOperational} />
      </div>

      <div className="nx-branch-card__contact">
        <span className="nx-inline">
          <Icon name="phone" size={13} />
          {branch.phone}
        </span>
        <span className="nx-inline">
          <Icon name="calendar" size={13} />
          {branch.schedule}
        </span>
      </div>

      <div className="nx-branch-card__stats">
        <div>
          <div className="nx-metric__label">Ventas de hoy</div>
          <div className="nx-metric__value">{formatCRC(branch.salesToday)}</div>
        </div>
        <div>
          <div className="nx-metric__label">Tiquetes</div>
          <div className="nx-metric__value">{formatNumber(branch.ticketsToday)}</div>
        </div>
        <div>
          <div className="nx-metric__label">Existencias administradas</div>
          <div className="nx-metric__value">{formatNumber(branch.managedProductCount)}</div>
        </div>
        <div>
          <div className="nx-metric__label">Stock bajo</div>
          <div
            className={`nx-metric__value${branch.lowStockCount > 0 ? ' nx-metric__value--warn' : ''}`}
          >
            {branch.lowStockCount}
          </div>
        </div>
        <div>
          <div className="nx-metric__label">Veterinarios activos</div>
          <div className="nx-metric__value">{branch.activeCollaborators}</div>
        </div>
        <div>
          <div className="nx-metric__label">Botiquines asignados</div>
          <div className="nx-metric__value">{branch.mobileKitsCount}</div>
        </div>
      </div>

      <div className="nx-branch-card__foot">
        <Link to={`/sucursales/${branch.id}`} className="nx-btn nx-btn--subtle" style={{ width: '100%', justifyContent: 'center' }}>
          Ver sucursal
          <span className="nx-btn__icon">
            <Icon name="chevron-right" size={16} />
          </span>
        </Link>
      </div>
    </article>
  );
}
