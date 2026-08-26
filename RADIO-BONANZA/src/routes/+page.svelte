<script lang="ts">
    import { onMount } from 'svelte';
    import { getCurrentSong } from '$lib/api/nrk-data.js';

    let { data } = $props();
    // svelte-ignore state_referenced_locally - svelte tror $derived passer bedre her fordi den ikke kan se at data.song faktisk brukes
    let currentSong = $state(data.currentSongPlaying);

    onMount(() => {
        const interval = setInterval(async () => {
            const newSong = await getCurrentSong();
            if (newSong?.title !== currentSong?.title) {
                console.log(`Changed from ${currentSong?.title} to ${newSong?.title}`);
                currentSong = newSong;
            }
        }, 1000);
        return () => clearInterval(interval);
    });

</script>

<h1>Radio Bonanza</h1>

<p>Current song: {currentSong ? `${currentSong.title}` : 'No song is currently playing.'}</p>

<p>{currentSong ? `By: ${currentSong.description}` : ''}</p>

<p>{currentSong ? `Duration: ${currentSong.duration}` : ''}</p>

<img src={currentSong?.imageUrl} alt={currentSong ? `${currentSong.title} by ${currentSong.description}` : ''} height="200" width="200" loading="lazy" />
