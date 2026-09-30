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

	function formatDate(iso: string) {
		return new Date(iso).toLocaleDateString("nb-NO", {
			day: "numeric",
			month: "long",
			year: "numeric"
		})
	}
</script>

<h1>Samlingen min</h1>

{#if isLoading}
	<Spinner />
{:else if error}
	<p>Klarte ikke hente gjenstander: {error}</p>
{:else if items.length === 0}
	<p>Det finnes ingen elementer enda.</p>
{:else}
	<ul class="items-grid">
		{#each items as item (item.id)}
			<li>
			<button type="button" class="ds-card" data-color="neutral">
				<div class="item-card">
				<div class="item-card-header">
					<h2 class="ds-heading" data-size="xs">{item.name}</h2>
					<span class="ds-tag" data-color="accent" data-size="sm">
					{item.category}
					</span>
				</div>
				<p class="ds-paragraph" data-size="sm">
					Lagt til {formatDate(item.addedAt)}
				</p>
				</div>
			</button>
			</li>
		{/each}
	</ul>
{/if}