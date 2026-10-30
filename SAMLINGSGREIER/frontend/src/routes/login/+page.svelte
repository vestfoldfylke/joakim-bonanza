<script lang="ts">
	import { goto } from "$app/navigation"
	import { API_URL } from "$lib/auth"
	import Spinner from "$lib/components/Spinner.svelte"

	let username = $state("")
	let password = $state("")
	let loginError = $state<string | null>(null)
	let isLoggingIn = $state(false)

	async function login(event: SubmitEvent) {
		event.preventDefault()
		loginError = null
		isLoggingIn = true
		try {
			const response = await fetch(`${API_URL}/auth/login`, {
				method: "POST",
				credentials: "include",
				headers: { "Content-Type": "application/json" },
				body: JSON.stringify({ username, password })
			})

			if (response.status === 401) {
				throw new Error("Feil brukernavn eller passord")
			}
			if (!response.ok) {
				throw new Error(`(POST) Fetch error while logging in: ${response.status}`)
			}

			password = ""
			await goto("/", { invalidateAll: true })
		} catch (e) {
			loginError = e instanceof Error ? e.message : "Ukjent feil"
		} finally {
			isLoggingIn = false
		}
	}
</script>

<div class="login-container">
	<h1 class="ds-heading">Logg inn</h1>

	<form onsubmit={login}>
		<ds-field class="ds-field">
			<label class="ds-label">
				Brukernavn
				<input class="ds-input" type="text" autocomplete="username" required bind:value={username} />
			</label>
		</ds-field>

		<ds-field class="ds-field">
			<label class="ds-label">
				Passord
				<input class="ds-input" type="password" autocomplete="current-password" required bind:value={password} />
			</label>
		</ds-field>

		{#if loginError}
			<div class="ds-alert" data-color="danger" role="alert">
				<p class="ds-paragraph">{loginError}</p>
			</div>
		{/if}

		{#if isLoggingIn}
			<Spinner label="Logger inn..." />
		{:else}
			<button class="ds-button" type="submit" data-variant="primary" disabled={!username || !password}>
				Logg inn
			</button>
		{/if}
	</form>
</div>

<style>
.login-container {
	max-width: 24rem;
	margin: var(--ds-size-18) auto;
}

form {
	display: flex;
	flex-direction: column;
	gap: var(--ds-size-4);
}
</style>
