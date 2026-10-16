export type Category = {
	id: string
	name: string
}

export function isCategory(value: unknown): value is Category {
	if (typeof value !== "object" || value === null) {
		return false
	}

	const candidate = value as Record<string, unknown>
	return typeof candidate.id === "string" && typeof candidate.name === "string"
}
