<script lang="ts">
  import { onMount } from 'svelte';
  import { currentSong } from '$lib/api/nrk_data';

  let { data } = $props();
  let song = $state(data.song);

  onMount(() => {
    const interval = setInterval(async () => {
      const nySang = await currentSong();
      if (nySang !== song) song = nySang;
    }, 1000);
    return () => clearInterval(interval);
  });
</script>

<h1>Radio Bonanza</h1>

<p>Current song: {song}</p>
