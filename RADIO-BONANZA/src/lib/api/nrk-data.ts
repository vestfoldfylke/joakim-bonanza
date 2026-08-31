type Program = {
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

export type ParsedProgram = Omit<Program, 'startTime'> & {
    startTime: Date,
    songEndTime: Date
};

function getConvertedTypeForEntry(entry: Program | undefined): ParsedProgram | undefined {
    if (!entry) return undefined;

    const startTimeMatch = entry.startTime.match(/Date\((\d+)/);
    const ms = Number(startTimeMatch?.[1]);
    const songStartDate = new Date(ms);

    const durationMatch = entry.duration.match(/PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?/);
    const hours = Number(durationMatch?.[1] ?? 0);
    const minutes = Number(durationMatch?.[2] ?? 0);
    const seconds = Number(durationMatch?.[3] ?? 0);
    const totalDurationInSeconds = hours * 3600 + minutes * 60 + seconds;

    const songEndDate = new Date(songStartDate.getTime() + totalDurationInSeconds * 1000)

    return {
        ...entry,
        startTime: songStartDate,
        songEndTime: songEndDate
    };
}

export async function fetchNrkData(): Promise<Program[]> {
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

export async function getLatestProgram(amount: number): Promise<ParsedProgram[] | undefined> {
    const program = await fetchNrkData();

    if (!program || program.length === 0) return undefined;

    const latestPlayed = program
        .filter((entry) => entry.relativeTimeType === 'Past')
        .slice(-amount);

    const parsedEntries = latestPlayed.map((entry) => getConvertedTypeForEntry(entry));

    return parsedEntries.filter((entry): entry is ParsedProgram => entry !== undefined);
}

export async function getCurrentPlaying(includeNews: boolean = false): Promise<ParsedProgram | undefined> {
    const program = await fetchNrkData();
    if (!program || program.length === 0) return undefined;

    let currentPlaying = program.find(entry => entry.relativeTimeType === 'Present' && entry.type === 'Music');

    if (!currentPlaying && includeNews) {
        currentPlaying = program.find(entry => entry.relativeTimeType === 'Present' && (entry.type === 'News' || entry.type === 'Music'));
    }

    if (!currentPlaying) return undefined;

    return getConvertedTypeForEntry(currentPlaying);
}
