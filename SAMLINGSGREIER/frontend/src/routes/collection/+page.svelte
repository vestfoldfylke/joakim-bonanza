<script lang="ts">
	import { onMount } from "svelte"
	import { PUBLIC_API_URL } from "$env/static/public"
	// components
	import Spinner from "$lib/components/Spinner.svelte"
	import { type Item, isItem } from "$lib/item"

	let CLEAN_API_URL = PUBLIC_API_URL.replace(/\/$/, "")

	let items = $state<Item[]>([])
	let loadError = $state<string | null>(null)
	let addError = $state<string | null>(null)
	let isLoading = $state(true)
	let isAdding = $state(false)

	let newItemName = $state("")
	let newItemCategory = $state("")
	let dialogEl = $state<HTMLDialogElement | undefined>()

	onMount(async () => {
		try {
			const response = await fetch(`${CLEAN_API_URL}/items`)
			if (!response.ok) {
				throw new Error(`(GET) Fetch error while getting items: ${response.status}`)
			}

			const data: unknown = await response.json()
			if (!Array.isArray(data) || !data.every(isItem)) {
				throw new Error("Uventet format på svar fra API-et")
			}
			items = data
		} catch (e) {
			loadError = e instanceof Error ? e.message : "Ukjent feil"
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

	async function addToCollection() {
		isAdding = true
		try {
			const response = await fetch(`${CLEAN_API_URL}/items`, {
				method: "POST",
				headers: { "Content-Type": "application/json" },
				body: JSON.stringify({ name: newItemName, category: newItemCategory })
			})

			if (!response.ok) {
				throw new Error(`(POST) Fetch error while adding items: ${response.status}`)
			}

			const created: unknown = await response.json()
			if (!isItem(created)) {
				throw new Error("Uventet format på svar fra API-et")
			}

			items.push(created)
			newItemName = ""
			newItemCategory = ""

			dialogEl?.close()
		} catch (e) {
			addError = e instanceof Error ? e.message : "Ukjent feil"
		} finally {
			isAdding = false
		}
	}
</script>

<h1>Samlingen min</h1>

{#if isLoading}
	<Spinner label="Laster inn..."/>
{:else if loadError}
	<div class="ds-alert" data-color="danger" role="alert">
		<h2 class="ds-heading" data-size="xs">En feil har oppstått</h2>
		<p class="ds-paragraph">Klarte ikke hente gjenstander: {loadError}</p>
	</div>
{:else if items.length === 0}
	<p>Det finnes ingen gjenstander enda.</p>
{:else}
	<div class="items-manager">
		<button
			class="ds-button"
			type="button"
			command="show-modal"
			commandfor="add-item-dialog"
			onclick={() => {addError = ""}}
		>
			<span class="material-symbols-outlined">add_card</span>
			Legg til ny
		</button>

		<dialog
			bind:this={dialogEl}
			class="ds-dialog"
			data-placement="center"
			data-modal="true"
			id="add-item-dialog"
		>
			<button
				class="ds-button"
				data-icon="true"
				commandfor="add-item-dialog"
				data-variant="tertiary"
				type="button"
				aria-label="Lukk dialogvindu"
				data-color="neutral"
				command="close"
			></button>
			<div class="ds-dialog__block">
				<h2 class="ds-heading">Legg til i samling</h2>
			</div>
			<ds-field class="ds-field">
				<label class="ds-label">
					Navn på ny oppføring
					<div class="ds-field-affixes">
						<input class="ds-input" type="text" bind:value={newItemName}/>
					</div>
				</label>
			</ds-field>

			<ds-field class="ds-field">
				<label class="ds-label">
					Velg en kategori
					<select class="ds-input" bind:value={newItemCategory}>
						<option value="" disabled selected>Velg en kategori…</option>
						<option value="Konsoller">Konsoller</option>
					</select>
				</label>
			</ds-field>

			<div class="ds-dialog__block">
				<div class="items-manager-dialog-action-buttons">
					{#if isAdding}
						<Spinner />
					{:else}
						<button
							class="ds-button"
							commandfor="add-item-dialog"
							data-variant="primary"
							type="button"
							data-color="success"
							onclick={addToCollection}
							disabled={!newItemName || !newItemCategory}
						>Legg til</button>
					{/if}
					<button
						class="ds-button"
						commandfor="add-item-dialog"
						data-variant="secondary"
						type="button"
						command="close"
					>Avbryt</button>
				</div>
				<br>
				{#if addError}
					<p
						class="ds-validation-message"
						data-field="validation"
						data-color="danger"
					>En feil oppstod! ({addError})</p>
				{/if}
			</div>
		</dialog>
	</div>

	<ul class="items-grid">
		{#each items as item (item.id)}
			<li>
				<div class="ds-card" data-color="neutral">
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
				</div>
			</li>
		{/each}
	</ul>
{/if}

<style>
.items-manager-dialog-action-buttons {
	display: flex;
	gap: var(--ds-size-4);
	margin-top: var(--ds-size-4);
}

.ds-dialog .ds-field {
	max-width: 20rem;
	padding: .5rem 1.5rem;
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
