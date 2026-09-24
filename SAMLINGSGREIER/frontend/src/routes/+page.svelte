<script lang="ts">
	import { onMount } from 'svelte';
	import type { Item } from '$lib/types';

    import { PUBLIC_API_URL } from '$env/static/public';

	let items = $state<Item[]>([]);
	let error = $state<string | null>(null);
    let loading = $state(true);

	onMount(async () => {
		try {
			const response = await fetch(`${PUBLIC_API_URL}/items`);
			if (!response.ok) throw new Error(`HTTP ${response.status}`);
			items = await response.json();
		} catch (e) {
			error = e instanceof Error ? e.message : 'Ukjent feil';
		} finally {
            loading = false;
        }
	});
</script>

<h1>Samlingen min</h1>

{#if loading}
    <p>Loading data..</p>
{:else if error}
	<p>Klarte ikke hente gjenstander: {error}</p>
{:else if items.length === 0}
    <p>Det finnes ingen elementer enda.</p>
{:else}
    <ul>
		{#each items as item (item.id)}
			<li>{item.name} ({item.category})</li>
		{/each}
	</ul>
{/if}
