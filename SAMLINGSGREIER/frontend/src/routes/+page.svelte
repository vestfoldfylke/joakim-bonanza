<script lang="ts">
  import { onMount } from "svelte"
  import { PUBLIC_API_URL } from "$env/static/public"
  import type { Item } from "$lib/types"
  import { isItem } from "$lib/types"

  let items = $state<Item[]>([])
  let error = $state<string | null>(null)
  let isLoading = $state(true)

  onMount(async () => {
    try {
      const response = await fetch(`${PUBLIC_API_URL}/items`)
      if (!response.ok) throw new Error(`HTTP ${response.status}`)

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
    <p>Henter data..</p>
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
