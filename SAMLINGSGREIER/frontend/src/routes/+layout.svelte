<script lang="ts">
	import favicon from "$lib/assets/favicon.svg"

	import "@digdir/designsystemet-web"
	import "@digdir/designsystemet-css"
	import "@digdir/designsystemet-css/theme"
	import "../style.css"

	let { children } = $props()
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

<div class="app-layout">
	<nav class=sidebar>
		<ul>
			<li>
				<a href="/">
					Hjem
				</a>
			</li>
			<li>
				<a href="#">
					Samlingen min
				</a>
			</li>
		</ul>
	</nav>

	<main>{@render children()}</main>
</div>

<style>
.sidebar {
	width: 16rem;
	flex-shrink: 0;
	padding: var(--ds-size-6);
	background-color: var(--ds-color-neutral-background-subtle);
	border-right: 1px solid var(--ds-color-neutral-border-subtle);
}

.sidebar ul {
	list-style: none;
	margin: 0;
	padding: 0;
	display: flex;
	flex-direction: column;
	gap: var(--ds-size-2);
}

.sidebar a {
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

.sidebar a:hover {
	background-color: var(--ds-color-neutral-surface-hover);
}

.sidebar a:focus-visible {
	outline: 2px solid var(--ds-color-accent-border-strong);
	outline-offset: 2px;
}
</style>