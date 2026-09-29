/**
 * Base URL for the API.
 *
 * VITE_API_BASE_URL overrides this in a .env file. Otherwise the port
 * from the current page origin is reused, so the API is reached on
 * whichever loopback address the browser resolved the page to. Hardcoding
 * a single host here is what breaks the app when the page is loaded from
 * 127.0.0.1 instead of localhost: the request would go to a different
 * origin than the one the CORS policy was written for.
 */
const fromEnv = import.meta.env.VITE_API_BASE_URL as string | undefined;

function resolveBaseUrl(): string {
  if (fromEnv) {
    return fromEnv;
  }

  if (typeof window !== "undefined" && window.location) {
    return `${window.location.protocol}//${window.location.hostname}:5000/api`;
  }

  return "http://localhost:5000/api";
}

export const API_BASE_URL = resolveBaseUrl();
