import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './styles/global.css';
import App from './App.tsx';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <a className="nx-skip-link" href="#contenido">
      Saltar al contenido
    </a>
    <App />
  </StrictMode>,
);
