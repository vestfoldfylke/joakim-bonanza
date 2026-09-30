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
				<button class="ds-button" type="button">
					<span class="material-symbols-outlined">add_card</span>
					Legg til ny
				</button>
			</li>
		</ul>
	</nav>

	<main>{@render children()}</main>
</div>