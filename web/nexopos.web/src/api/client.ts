// Capa centralizada de acceso HTTP. La URL base del API es configurable mediante
// la variable de entorno VITE_API_BASE_URL (ver .env.example).

const rawBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5257';
const baseUrl = rawBaseUrl.replace(/\/+$/, '');

export class ApiError extends Error {
  readonly status: number;

  constructor(message: string, status: number) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
  }
}

export async function getJson<T>(path: string, signal?: AbortSignal): Promise<T> {
  const url = `${baseUrl}${path.startsWith('/') ? path : `/${path}`}`;

  let response: Response;
  try {
    response = await fetch(url, {
      headers: { Accept: 'application/json' },
      signal,
    });
  } catch (cause) {
    if (cause instanceof DOMException && cause.name === 'AbortError') {
      throw cause;
    }
    throw new ApiError(
      'No se pudo conectar con el servidor. Verifique que el API esté en ejecución.',
      0,
    );
  }

  if (!response.ok) {
    throw new ApiError(
      `El servidor respondió con un error (${response.status}).`,
      response.status,
    );
  }

  return (await response.json()) as T;
}
