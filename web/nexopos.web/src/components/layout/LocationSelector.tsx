import { useState } from 'react';
import { Icon } from '../ui/Icon';

const OPTIONS = [
  'Todas las sucursales',
  'Veterinaria San José',
  'Veterinaria Heredia',
  'Veterinaria Cartago',
  'Botiquines móviles',
];

/**
 * Selector visual de ubicación. En esta iteración es un control de contexto
 * informativo (no filtra todavía cada pantalla); prepara el patrón de
 * navegación multi-sucursal que usarán los próximos módulos.
 */
export function LocationSelector() {
  const [value, setValue] = useState(OPTIONS[0]);

  return (
    <label className="nx-location-selector">
      <Icon name="pin" size={16} />
      <select
        className="nx-location-selector__select"
        value={value}
        onChange={(event) => setValue(event.target.value)}
        aria-label="Ubicación"
      >
        {OPTIONS.map((option) => (
          <option key={option} value={option}>
            {option}
          </option>
        ))}
      </select>
      <Icon name="chevron-down" size={14} className="nx-location-selector__chevron" />
    </label>
  );
}
