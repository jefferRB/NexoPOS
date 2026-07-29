import { useCallback, useEffect, useState } from 'react';

type Fetcher<T> = (signal: AbortSignal) => Promise<T>;

export interface ApiDataState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
  reload: () => void;
}

/**
 * Hook genérico de carga de datos del API. Maneja los estados de carga y error,
 * cancela la petición al desmontar y expone una función para reintentar.
 * El `fetcher` debe ser estable (envuélvalo en useCallback si depende de props).
 */
export function useApiData<T>(fetcher: Fetcher<T>): ApiDataState<T> {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [attempt, setAttempt] = useState(0);

  const reload = useCallback(() => setAttempt((value) => value + 1), []);

  useEffect(() => {
    const controller = new AbortController();
    let active = true;

    setLoading(true);
    setError(null);

    fetcher(controller.signal)
      .then((result) => {
        if (active) {
          setData(result);
        }
      })
      .catch((cause: unknown) => {
        if (!active || controller.signal.aborted) {
          return;
        }
        setData(null);
        setError(cause instanceof Error ? cause.message : 'Ocurrió un error inesperado.');
      })
      .finally(() => {
        if (active) {
          setLoading(false);
        }
      });

    return () => {
      active = false;
      controller.abort();
    };
  }, [fetcher, attempt]);

  return { data, loading, error, reload };
}
