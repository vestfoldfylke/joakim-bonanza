import fetchNrkData, { currentSong } from '$lib/api/nrk_data';

export const load = async () => {
  const song = await currentSong();
  return { song };
};

