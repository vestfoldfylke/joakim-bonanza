<script lang="ts">
	import favicon from "$lib/assets/favicon.svg"

	import "@digdir/designsystemet-web"
	import "@digdir/designsystemet-css"
	import "@digdir/designsystemet-css/theme"
	import "../style.css"
	import { goto } from "$app/navigation"
	import { API_URL } from "$lib/auth"

	let { data, children } = $props()

	async function logout() {
		await fetch(`${API_URL}/auth/logout`, { method: "POST", credentials: "include" })
		await goto("/login", { invalidateAll: true })
	}
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

{#if data.user}
	<div class="app-layout">
		<div class="sidebar">
			<div class="sidebar-header">
				<nav class="nav-primary">
					<div class="logo-container ds-paragraph">
						Samlingsgreier
					</div>
					<a href="/">
						Hjem
					</a>
					<a href="/collection">
						Samlingen min
					</a>
				</nav>
			</div>

			<div class="user-menu">
					<hr aria-hidden="true" class="ds-divider"/>
					<div class="user-menu-item">
						<button class="ds-button user-menu-button"
							type="button"
							data-variant="tertiary"
							data-color="neutral"
							popovertarget="user-menu-dropdown"
						>
							<span
								aria-hidden="true"
								class="ds-avatar"
								data-size="xs"
								data-color="accent"
							></span>
							<span class="ds-paragraph user-menu-button-title" data-size="sm"><strong>{data.user?.username}</strong></span>
							<span class="material-symbols-outlined user-menu-chevron" aria-hidden="true">keyboard_arrow_up</span>
						</button>

						<div
							id="user-menu-dropdown"
							class="ds-dropdown"
							popover="auto"
							data-placement="top-start"
						>
							<ul class="user-menu-list" aria-labelledby="user-menu-heading">
								<li>
									<a class="ds-button user-menu-link"
										data-variant="tertiary"
										data-color="neutral"
										data-size="sm"
										href="/settings"
									>
										<span class="material-symbols-outlined" aria-hidden="true">settings</span>
										Instillinger
									</a>
								</li>
								<li>
									<button class="ds-button user-menu-link"
										data-variant="tertiary"
										data-color="neutral"
										data-size="sm"
										onclick={logout}
									>
										<span class="material-symbols-outlined" aria-hidden="true">logout</span>
										Logg ut
								</button>
								</li>
							</ul>
						</div>
					</div>
				</div>
		</div>

		<main>{@render children()}</main>
	</div>
{:else}
	<main>{@render children()}</main>
{/if}

<style>
.sidebar {
	box-sizing: border-box;
	display: flex;
    flex-direction: column;
    width: 20rem;
	position: sticky;
	padding: 0 0.5rem;
	background-color: var(--ds-color-neutral-background-subtle);
	border-right: 1px solid var(--ds-color-neutral-border-subtle);
}

.logo-container {
    font-weight: 600;
    font-size: 2rem;
    padding: 1rem 1rem 1rem .5rem;
}

.nav-primary {
    flex-shrink: 0;
    padding-bottom: 2rem;
}

.nav-primary a {
	display: block;
	color: var(--ds-color-neutral-text-default);
	text-decoration: none;
	padding: var(--ds-size-2) var(--ds-size-3);
	border-radius: var(--ds-border-radius-md);
	font-weight: 500;
	text-underline-offset: 4px;
	text-decoration-thickness: 2px;
	transition: background-color 0.15s;
}

.nav-primary a:hover {
	background-color: var(--ds-color-neutral-surface-hover);
}

.nav-primary a:focus-visible {
	outline: 2px solid var(--ds-color-accent-border-strong);
	outline-offset: 2px;
}

.user-menu-item {
    display: flex;
}

.user-menu-button-title {
    font-size: 0.85rem;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
	flex: 1;
    text-align: left;
}

.user-menu {
    flex-shrink: 0;
	margin-top: auto;
	padding: 0.5rem 0;
}

.user-menu-button {
    flex: 1;
    min-width: 0;
    justify-content: flex-start;
    gap: var(--ds-size-2);
    padding: var(--ds-size-2);
}

.user-menu-button .ds-avatar {
    --dsc-avatar-size: 1.7rem;
}

.user-menu-chevron {
    transition: transform 0.15s;
}

.user-menu-item:has(.ds-dropdown:popover-open) .user-menu-chevron {
    transform: rotate(180deg);
}

.user-menu-list {
    margin: 0;
    padding: 0;
    list-style: none;
}

.user-menu-link {
    width: 100%;
    justify-content: flex-start;
    gap: var(--ds-size-2);
}

</style>
