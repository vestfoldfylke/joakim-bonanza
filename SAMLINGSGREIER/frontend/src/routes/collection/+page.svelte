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
	<Spinner hasLabel={true}/>
{:else if error}
	<div class="ds-alert" data-color="danger" role="alert">
		<h2 class="ds-heading" data-size="xs" style="margin-bottom:var(--ds-size-2)">En feil har oppstått</h2>
		<p class="ds-paragraph">Klarte ikke hente gjenstander: {error}</p>
	</div>
{:else if items.length === 0}
	<p>Det finnes ingen elementer enda.</p>
{:else}
	<div class="items-manager">
		<button class="ds-button" type="button">
			<span class="material-symbols-outlined">add_card</span>
			Legg til ny
		</button>

		<button class="ds-button" type="button" data-variant="secondary">
			<span class="material-symbols-outlined">sort</span>
			Sorter
		</button>
	</div>

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

<style>
.items-manager {
	list-style: none;
	padding: 0;
	margin: var(--ds-size-6) 0 0;
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(9rem, 1fr));
	gap: var(--ds-size-1);
}

.items-grid {
	list-style: none;
	padding: 0;
	margin: var(--ds-size-6) 0 0;
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(18rem, 1fr));
	gap: var(--ds-size-4);
}

.items-grid > li {
	display: contents;
}

.items-grid .ds-card {
	width: 100%;
	text-align: left;
	cursor: pointer;
	transition:
		transform 0.15s ease,
		box-shadow 0.15s ease;
}

.items-grid .ds-card:hover {
	transform: translateY(-2px);
	box-shadow: 0 4px 12px rgb(0 0 0 / 0.08);
}

.item-card {
	display: flex;
	flex-direction: column;
	gap: var(--ds-size-3);
	padding: var(--ds-size-4);
}

.item-card-header {
	display: flex;
	justify-content: space-between;
	align-items: flex-start;
	gap: var(--ds-size-3);
}

.item-card-header h2 {
	margin: 0;
}

.item-card p {
	margin: 0;
	color: var(--ds-color-neutral-text-subtle);
}
</style>
