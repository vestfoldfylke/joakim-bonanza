type NrkElement = {
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

type ParsedNrkElement = Omit<NrkElement, 'startTime'> & {
    startTime: Date,
    songEndTime: Date
};


export async function fetchNrkData(): Promise<NrkElement[]> {
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

export async function getCurrentSong(): Promise<ParsedNrkElement | undefined> {
    const program = await fetchNrkData();

    if (!program || program.length === 0) {
        return undefined;
    }

    const currentSong = program.find(song => song.relativeTimeType === 'Present' && song.type === 'Music');
    if (!currentSong) {
        return undefined;
    }

    const startTimeMatch = currentSong.startTime.match(/Date\((\d+)/);
    const ms = Number(startTimeMatch?.[1]); // "1787650985000" -> 1787650985000
    const startTime = new Date(ms);

    const durationMatch = currentSong?.duration.match(/PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?/);
    const hours = Number(durationMatch?.[1] ?? 0);
    const minutes = Number(durationMatch?.[2] ?? 0);
    const seconds = Number(durationMatch?.[3] ?? 0);
    const totalDurationInSeconds = hours * 3600 + minutes * 60 + seconds;

    const songEndDate = new Date(startTime.getTime() + totalDurationInSeconds * 1000)

    return {
        ...currentSong,
        startTime: startTime,
        songEndTime: songEndDate
    };
}