import { redirect } from "@sveltejs/kit"
import { getCurrentUser } from "$lib/auth"
import type { LayoutLoad } from "./$types"

export const ssr = false

export const load: LayoutLoad = async ({ fetch, url }) => {
	const user = await getCurrentUser(fetch)

	if (!user && url.pathname !== "/login") {
		redirect(307, "/login")
	}

	if (user && url.pathname === "/login") {
		redirect(307, "/")
	}

	return { user }
}
