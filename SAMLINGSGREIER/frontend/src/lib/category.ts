export type Category = {
	id: string
	name: string
}

export function isCategory(value: unknown): value is Category {
	if (value === null || typeof value !== "object") {
		return false
	}

	const candidate = value as Category
	return typeof candidate.id === "string" && typeof candidate.name === "string"
}
