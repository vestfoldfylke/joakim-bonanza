export type Item = {
  id: string
  name: string
  category: string
  addedAt: string
}

export function isItem(value: unknown): value is Item {
  if (typeof value !== "object" || value === null) return false
  const candidate = value as Record<string, unknown>
  return typeof candidate.id === "string" && typeof candidate.name === "string" && typeof candidate.category === "string" && typeof candidate.addedAt === "string"
}
