/**
 * Formats a movie date for display.
 *
 * The API serialises `DateTime` with System.Text.Json, which emits ISO-8601
 * (e.g. `2026-09-29T10:10:44.9925205`). `new Date()` parses that directly,
 * and because the value carries a time component the browser resolves it to
 * an exact instant and renders it in the reader's own timezone - which is
 * the behaviour you want for a "when was this added" column.
 *
 * Truncating the value to `yyyy-MM-dd` before it gets here would be wrong:
 * a date-only string is defined as UTC midnight, so every reader west of
 * Greenwich would be shown the previous day.
 *
 * The legacy branch below exists only for the older `MM/dd/yyyy HH:mm:ss`
 * shape that `DateTime.ToString()` produces. `new Date()` cannot be trusted
 * on that format in every engine, so its parts are read out individually and
 * fed to the constructor, which builds a local-time date and never shifts
 * the rendered day.
 */
const LEGACY_SERVER_FORMAT =
  /^(\d{1,2})\/(\d{1,2})\/(\d{4})(?:[ T](\d{1,2}):(\d{2})(?::(\d{2}))?)?$/;

export function formatDate(value: string | undefined): string {
  if (!value) {
    return "-";
  }

  const parsed = parse(value.trim());
  if (!parsed) {
    // Unrecognised: show it verbatim rather than "Invalid Date".
    return value;
  }

  return parsed.toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

function parse(value: string): Date | undefined {
  const legacy = LEGACY_SERVER_FORMAT.exec(value);
  if (legacy) {
    const [, month, day, year, hour, minute, second] = legacy;
    return new Date(
      Number(year),
      Number(month) - 1,
      Number(day),
      Number(hour ?? 0),
      Number(minute ?? 0),
      Number(second ?? 0)
    );
  }

  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? undefined : parsed;
}