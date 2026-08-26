interface NrkElement {
    title: string,
    description: string,
    programId: string,
    channelId: string,
    startTime: string,
    duration: string,
    type: string,
    imageUrl: string | null,
    programTitle: string,
    relativeTimeType: string,
    category: string,
    contributors: string,
    creators: string | null;
}

export default async function fetchNrkData(): Promise<NrkElement[]> {
    try {
        const response = await fetch("https://psapi.nrk.no/channels/p3musikk/liveelements");
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }

        const result = await response.json();
        return result;
    } catch (error) {
        console.error((error as Error).message);
        return [];
    }
}

export async function currentSong(): Promise<NrkElement | undefined> {
    const program = await fetchNrkData();

    if (!program || program.length === 0) {
        return undefined;
    }

    for (let song of program) {
        if (song.relativeTimeType === 'Present' && song.type === 'Music') {
            return song;
        }
    }

    return undefined;
}