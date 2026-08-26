import fetchNrkData, { currentSong } from '$lib/api/nrk_data';
import { onMount } from 'svelte';

export const load = async () => {
  const song = await currentSong();
  return { song };
};

