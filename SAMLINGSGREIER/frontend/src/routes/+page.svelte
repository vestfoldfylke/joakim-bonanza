<script lang="ts">
	import { onMount } from 'svelte';
	import type { Item } from '$lib/types';
    
	const API_URL = 'http://localhost:5199';

	let items = $state<Item[]>([]);
	let error = $state<string | null>(null);

	onMount(async () => {
		try {
			const response = await fetch(`${API_URL}/items`);
			if (!response.ok) throw new Error(`HTTP ${response.status}`);
			items = await response.json();
		} catch (e) {
			error = e instanceof Error ? e.message : 'Ukjent feil';
		}
	});
</script>

<h1>Samlingen min</h1>

{#if error}
	<p>Klarte ikke hente gjenstander: {error}</p>
{:else}
	<ul>
		{#each items as item (item.id)}
			<li>{item.name} ({item.category})</li>
		{/each}
	</ul>
{/if}
