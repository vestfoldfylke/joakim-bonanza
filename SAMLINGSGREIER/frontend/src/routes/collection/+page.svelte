<script lang="ts">
	import { onMount } from "svelte"
	import { PUBLIC_API_URL } from "$env/static/public"
	import { type Category, isCategory } from "$lib/category"
	// components
	import Spinner from "$lib/components/Spinner.svelte"
	import { type Item, isItem } from "$lib/item"

	let CLEAN_API_URL = PUBLIC_API_URL.replace(/\/$/, "")

	let categories = $state<Category[]>([])
	let loadError = $state<string | null>(null)
	let addError = $state<string | null>(null)
	let isLoading = $state(true)
	let isRemoving = $state(false)
	let isAdding = $state(false)
	let removeError = $state<string | null>(null)

	let newItemName = $state("")
	let newItemCategory = $state("")
	let dialogEl = $state<HTMLDialogElement | undefined>()

	let searchFilter = $state("");
	
	let items = $state<Item[]>([])

	const sortOptions = [
		{ value: "date", label: "Dato lagt til" },
		{ value: "name", label: "Navn" },
		{ value: "category", label: "Kategori" }
	]
	let sortBy = $state("date")
	
	const visibleItems = $derived(
		items
			.filter((item) => item.name.toLowerCase().includes(searchFilter.trim().toLowerCase()))
			.toSorted((a, b) => {
				if (sortBy === "name") {
					return a.name.localeCompare(b.name, "nb")
				}
				if (sortBy === "category") {
					return categoryName(a.categoryId).localeCompare(categoryName(b.categoryId), "nb")
				}
				return new Date(b.addedAt).getTime() - new Date(a.addedAt).getTime()
			})
	)

	onMount(async () => {
		try {
			const [itemsRes, categoriesRes] = await Promise.all([fetch(`${CLEAN_API_URL}/items`), fetch(`${CLEAN_API_URL}/categories`)])
			if (!itemsRes.ok) {
				throw new Error(`(GET) Fetch error while getting items: ${itemsRes.status}`)
			}
			if (!categoriesRes.ok) {
				throw new Error(`(GET) Fetch error while getting categories: ${categoriesRes.status}`)
			}

			const itemData: unknown = await itemsRes.json()
			const categoryData: unknown = await categoriesRes.json()
			if (!Array.isArray(itemData) || !itemData.every(isItem) || !Array.isArray(categoryData) || !categoryData.every(isCategory)) {
				throw new Error("Uventet format på svar fra API-et")
			}
			items = itemData
			categories = categoryData
		} catch (e) {
			loadError = e instanceof Error ? e.message : "Ukjent feil"
		} finally {
			isLoading = false
		}
	})

	function categoryName(id: string) {
		return categories.find((c) => c.id === id)?.name ?? "Ukjent kategori"
	}

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
				body: JSON.stringify({ name: newItemName, categoryId: newItemCategory })
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

	async function removeFromCollection(Id: string) {
		removeError = null
		isRemoving = true
		try {
			const response = await fetch(`${CLEAN_API_URL}/items/${Id}`, {
				method: "DELETE"
			})

			if (!response.ok) {
				throw new Error(`(DELETE) Fetch error while removing items: ${response.status}`)
			}

			items = items.filter((item) => item.id !== Id)
		} catch (e) {
			removeError = e instanceof Error ? e.message : "Ukjent feil"
		} finally {
			isRemoving = false
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
{:else}
	<div class="items-manager">
		<ds-suggestion class="ds-search search-field">
			<input class="ds-input" type="search" aria-label="Søk" bind:value={searchFilter}/>
		</ds-suggestion>

		<button
			class="ds-button"
			type="button"
			data-variant="secondary"
			popovertarget="sort-menu"
		>
			<span class="material-symbols-outlined" aria-hidden="true">sort</span>
			Sorter
		</button>
		<div class="ds-dropdown sort-menu" data-placement="bottom-end" id="sort-menu" popover>
			<ul aria-label="Velg rekkefølge">
				{#each sortOptions as option (option.value)}
					<li>
						<button
							class="ds-button"
							type="button"
							data-variant="tertiary"
							data-size="xs"
							popovertarget="sort-menu"
							popovertargetaction="hide"
							onclick={() => (sortBy = option.value)}
						>
							<span class="material-symbols-outlined sort-menu-check" aria-hidden="true">
								{sortBy === option.value ? "check" : ""}
							</span>
							{option.label}
						</button>
					</li>
				{/each}
			</ul>
		</div>

		<button
			class="ds-button"
			type="button"
			command="show-modal"
			commandfor="add-item-dialog"
			onclick={() => {addError = null}}
		>
			<span class="material-symbols-outlined">add_card</span>
			Legg til
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
						{#each categories as category (category.id)}
							<option value={category.id}>{category.name}</option>
						{/each}
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
	{#if removeError}
		<br/>
		<div class="ds-alert remove-error-alert" data-color="danger" role="alert">
			<button
				class="ds-button remove-error-alert__close"
				type="button"
				data-variant="tertiary"
				data-color="neutral"
				data-icon="true"
				data-size="sm"
				aria-label="Lukk varsel"
				onclick={() => (removeError = null)}
			>
				<span class="material-symbols-outlined" aria-hidden="true">close</span>
			</button>
			<h2 class="ds-heading" data-size="xs">En feil har oppstått!</h2>
			<p class="ds-paragraph">Klarte ikke å fjerne gjenstander: {removeError}</p>
		</div>
	{/if}

	{#if !(items.length === 0)}
		{#if isRemoving}
			<br/>
			<Spinner label="Fjerner gjenstander..."/>
		{:else}
		<ul class="items-grid">
			{#each visibleItems as item (item.id)}
				<li>
					<div class="ds-card" data-color="neutral">
						<div class="item-card">
							<div class="item-card-header">
								<h2 class="ds-heading" data-size="xs" title={item.name}>{item.name}</h2>
								<span class="ds-tag" data-color="accent" data-size="sm">
									{categoryName(item.categoryId)}
								</span>	
								<button
									type="button"
									class="ds-button"
									data-variant="tertiary"
									data-size="sm"
									data-icon
									popovertarget={`menu-${item.id}`}
									data-tooltip="Behandle"
									aria-label="Behandle"
								>
									<span class="material-symbols-outlined" aria-hidden="true">more_vert</span>
								</button>
								<div class="ds-dropdown" data-placement="bottom-end" id={`menu-${item.id}`} popover>
									<ul>
										<li>
											<button
												type="button"
												data-size="sm"
												class="ds-button"
												data-variant="tertiary"
												onclick={() => removeFromCollection(item.id)}>
												<span class="material-symbols-outlined" aria-hidden="true">delete</span>
												Slett
											</button>
										</li>
									</ul>
								</div>
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
	{:else}
		{#if !isRemoving}
			<p>Her var det tomt..</p>
		{:else}
			<br/>
			<Spinner label="Fjerner gjenstander..."/>
		{/if}
	{/if}
{/if}

<style>
.remove-error-alert {
	position: relative;
}

.remove-error-alert__close {
	position: absolute;
	top: var(--ds-size-2);
	right: var(--ds-size-2);
}

.items-manager {
	display: flex;
	gap: var(--ds-size-2);
}
.sort-menu {
	min-width: 18rem;
}

.sort-menu-check {
	width: 1.5rem;
}

.search-field {
	flex: 1;
	max-width: 30rem;
}

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
	gap: var(--ds-size-2);
	padding: var(--ds-size-1);
}

.item-card-header {
	display: flex;
	justify-content: space-between;
	align-items: flex-start;
	gap: var(--ds-size-3);
}

.item-card-header h2 {
	margin: 0;
	flex: 1;
	min-width: 0;
	overflow-wrap: anywhere;
	display: -webkit-box;
	-webkit-box-orient: vertical;
	-webkit-line-clamp: 2;
	line-clamp: 2;
	overflow: hidden;
}

.item-card-header .ds-tag,
.item-card-header .ds-button {
	flex-shrink: 0;
}

.item-card-header .ds-tag {
	white-space: nowrap;
}

.item-card p {
	margin: 0;
	color: var(--ds-color-neutral-text-subtle);
}
</style>
