<script lang="ts">
	import '@digdir/designsystemet-css';
	import '@digdir/designsystemet-css/theme';
	import "../style.css"
	import favicon from '$lib/assets/favicon.svg';

	let { children } = $props();
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

{@render children()}
