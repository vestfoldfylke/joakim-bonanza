import { PUBLIC_API_URL } from "$env/static/public"

export const API_URL = PUBLIC_API_URL.replace(/\/$/, "")

export type CurrentUser = {
	username: string
	role: string
}

export function isCurrentUser(value: unknown): value is CurrentUser {
	if (typeof value !== "object" || value === null) {
		return false
	}

	const candidate = value as Record<string, unknown>
	return typeof candidate.username === "string" && typeof candidate.role === "string"
}

export async function getCurrentUser(fetchFn: typeof fetch): Promise<CurrentUser | null> {
	const res = await fetchFn(`${API_URL}/auth/me`, { credentials: "include" })
	if (res.status === 401) {
		return null
	}
	if (!res.ok) {
		throw new Error(`(GET) Fetch error while getting current user: ${res.status}`)
	}

	const data: unknown = await res.json()
	return isCurrentUser(data) ? data : null
}
