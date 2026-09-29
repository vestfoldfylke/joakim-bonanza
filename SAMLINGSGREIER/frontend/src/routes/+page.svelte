<script lang="ts">
	import { onMount } from "svelte"
	import { PUBLIC_API_URL } from "$env/static/public"
	// components
	import Spinner from "$lib/components/Spinner.svelte"
	import { type Item, isItem } from "$lib/types"

	let items = $state<Item[]>([])
	let error = $state<string | null>(null)
	let isLoading = $state(true)

	onMount(async () => {
		try {
			const response = await fetch(`${PUBLIC_API_URL.replace(/\/$/, "")}/items`)
			if (!response.ok) {
				throw new Error(`HTTP ${response.status}`)
			}

			const data: unknown = await response.json()
			if (!Array.isArray(data) || !data.every(isItem)) {
				throw new Error("Uventet format på svar fra API-et")
			}
			items = data
		} catch (e) {
			error = e instanceof Error ? e.message : "Ukjent feil"
		} finally {
			isLoading = false
		}
	})
</script>

<h1>Samlingen min</h1>

{#if isLoading}
	<Spinner />
{:else if error}
	<p>Klarte ikke hente gjenstander: {error}</p>
{:else if items.length === 0}
	<p>Det finnes ingen elementer enda.</p>
{:else}
	<ul>
		{#each items as item (item.id)}
			<button type="button" class="ds-card" data-color="neutral">
				<div class="ds-card__block">
					<h2 class="ds-heading">{item.name} ({item.category})</h2>
				</div>
				<div class="ds-card__block">
					<p class="ds-paragraph">Kategori: {item.category}</p>
					<p class="ds-paragraph">Opprettet: {new Date(item.addedAt).toLocaleString()}</p>
					<p class="ds-paragraph">Id: {item.id}</p>
				</div>
			</button>
		{/each}
	</ul>
{/if}