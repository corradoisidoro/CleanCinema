/**
 * Formats a movie date for display.
 *
 * The API returns an invariant-culture `MM/dd/yyyy HH:mm:ss` string, which
 * is not a format `new Date()` reliably parses. The components are read out
 * and passed to the Date constructor individually, which builds a local-time
 * date and therefore never shifts the rendered day.
 *
 * ISO-8601 input is also accepted, so the helper keeps working if the API
 * later switches to `DateTime` JSON serialisation.
 */
const SERVER_FORMAT = /^(\d{1,2})\/(\d{1,2})\/(\d{4})(?:[ T](\d{1,2}):(\d{2})(?::(\d{2}))?)?$/;

export function formatDate(value: string | undefined): string {
  if (!value) {
    return "—";
  }

  const match = SERVER_FORMAT.exec(value.trim());
  if (!match) {
    // Not the server format: try a standard parse before giving up.
    const fallback = new Date(value);
    if (Number.isNaN(fallback.getTime())) {
      return value;
    }
    return fallback.toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  }

  const [, month, day, year, hour, minute, second] = match;
  const parsed = new Date(
    Number(year),
    Number(month) - 1,
    Number(day),
    Number(hour ?? 0),
    Number(minute ?? 0),
    Number(second ?? 0)
  );

  if (Number.isNaN(parsed.getTime())) {
    return value;
  }

  return parsed.toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}
