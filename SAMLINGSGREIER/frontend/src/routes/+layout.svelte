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

{@render children()}
