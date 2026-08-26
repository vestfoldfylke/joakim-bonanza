import getNrkData from '$lib/api/nrk';

export const load = async () => {
    const result = await getNrkData();
    
    for (let song of result) {
        if (song.relativeTimeType === 'Present' && song.type === 'Music') {
            console.log(`Song playing now: ${song.title} by ${song.description}`);
        }
    }
};
