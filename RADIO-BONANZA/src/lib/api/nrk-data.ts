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

export type ParsedNrkElement = Omit<NrkElement, 'startTime'> & {
    startTime: Date,
    songEndTime: Date
};

async function getParsedType(song: NrkElement | undefined): Promise<ParsedNrkElement | undefined> {
    if (!song) return undefined;

    const startTimeMatch = song?.startTime.match(/Date\((\d+)/);
    const ms = Number(startTimeMatch?.[1]);
    const songStartDate = new Date(ms);

    const durationMatch = song?.duration.match(/PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?/);
    const hours = Number(durationMatch?.[1] ?? 0);
    const minutes = Number(durationMatch?.[2] ?? 0);
    const seconds = Number(durationMatch?.[3] ?? 0);
    const totalDurationInSeconds = hours * 3600 + minutes * 60 + seconds;

    const songEndDate = new Date(songStartDate.getTime() + totalDurationInSeconds * 1000)

    return {
        ...song,
        startTime: songStartDate,
        songEndTime: songEndDate
    };
}

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

export async function getLatestSongs(amount: number): Promise<ParsedNrkElement[] | undefined> {
    const program = await fetchNrkData();

    if (!program || program.length === 0) return undefined;

    const latestSongs = program
        .filter((song) => song.type === 'News' && song.relativeTimeType === 'Past')
        .slice(-amount);

    const parsedSongs = await Promise.all(latestSongs.map((song) => getParsedType(song)));

    return parsedSongs.filter((song): song is ParsedNrkElement => song !== undefined);
}

export async function getCurrentPlaying(includesNews: boolean = false): Promise<ParsedNrkElement | undefined> {
    const program = await fetchNrkData();
    if (!program || program.length === 0) return undefined;

    let currentPlaying = program.find(song => song.relativeTimeType === 'Present' && song.type === 'Music');

    if (!currentPlaying && includesNews) {
        currentPlaying = program.find(song => song.relativeTimeType === 'Present' && song.type === 'News');
    }

    if (!currentPlaying) return undefined;

    return getParsedType(currentPlaying);
}