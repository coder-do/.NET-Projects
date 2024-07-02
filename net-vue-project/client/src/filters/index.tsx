// src/filters.js
export function price(value: number) {
  if (typeof value !== "number") {
    return value;
  }
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  }).format(value);
}

export function date(isoString: Date) {
  const date = new Date(isoString);
  const options: any = { year: 'numeric', month: 'long', day: 'numeric' };
  return date.toLocaleDateString("en-US", options);
}
