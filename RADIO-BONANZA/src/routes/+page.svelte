<script lang="ts">
    import { onMount } from 'svelte';
    import { getCurrentPlaying, getLatestLiveElements, type DetailedRadioLiveElement } from '$lib/api/nrk-data.js';
    import fallbackImage from '$lib/assets/nrkp3-logo.jpg';
    import { env } from '$env/dynamic/public';

    let progressPercent = $state(0);
    let currentSongTime = $state(0);

    // Config
    const parsedLastSongsAmount = Number(env.PUBLIC_PAST_SONGS_AMOUNT);
    const isValidLastSongsAmount = Number.isInteger(parsedLastSongsAmount) && parsedLastSongsAmount > 0;

    if (env.PUBLIC_PAST_SONGS_AMOUNT !== undefined && !isValidLastSongsAmount) {
        console.warn(`PUBLIC_PAST_SONGS_AMOUNT must be a positive integer, got "${env.PUBLIC_PAST_SONGS_AMOUNT}". Falling back to 3.`);
    }

    const pastSongsAmount = isValidLastSongsAmount ? parsedLastSongsAmount : 3;

    const ALARM_ARTIST = 'Sondre Lerche';

    let currentSong = $state<DetailedRadioLiveElement>();
    let lastFewSongs = $state<DetailedRadioLiveElement[]>();
    let lastFetchedTitle = $state<string>();
    let formattedElapsed = $state('00:00:00');
    let formattedTotal = $state('00:00:00');

    let isAlarmArtist = $derived(
        currentSong?.description?.toLowerCase().includes(ALARM_ARTIST.toLowerCase()) ?? false
    );

    function updateProgressBar(): void {
        if (!currentSong) return;

        const now = Date.now();
        const start = currentSong.startTime.getTime();
        const end = currentSong.songEndTime.getTime();

        if (now >= end && currentSong.type === 'Music') {
            currentSong = undefined;
            progressPercent = 0;
            currentSongTime = 0;
            formattedElapsed = '00:00:00';
            formattedTotal = '00:00:00';
            return;
        }

        currentSongTime = now - start;
        progressPercent = Math.min(100, Math.max(0, ((now - start) / (end - start)) * 100));
        formattedElapsed = new Date((Math.floor(currentSongTime / 1000)) * 1000).toISOString().substr(11, 8);
        formattedTotal = new Date((Math.floor((end - start) / 1000)) * 1000).toISOString().substr(11, 8);
    }

    onMount(() => {
        let interval: ReturnType<typeof setInterval>;

        async function refresh() {
            updateProgressBar();
            const newSong = await getCurrentPlaying(true);

            if (newSong?.title !== lastFetchedTitle) {
                console.log(`Changed from ${lastFetchedTitle} to ${newSong?.title}`);
                lastFewSongs = [...(await getLatestLiveElements(pastSongsAmount) ?? [])].reverse();
                lastFetchedTitle = newSong?.title;
                currentSong = newSong;

                updateProgressBar();
            }
        }

        refresh().then(() => {
            interval = setInterval(refresh, 1000);
        });

        return () => clearInterval(interval);
    });


</script>

<div class="main">
    {#if isAlarmArtist}
        <div class="ds-alert" data-color="danger" role="alert">
            <h2 class="ds-heading" data-size="xs" style="margin-bottom:var(--ds-size-2)">Advarsel til Rune!!</h2>
            <p class="ds-paragraph">{ALARM_ARTIST.toWellFormed()} spilles nå!!!!!! skru ned volumet på radio'n 🤢</p>
        </div>
    {/if}

    <div class="player ds-card" data-variant="default">

        <div class="player-top">
            <img src={currentSong?.imageUrl || fallbackImage} alt={currentSong ? `${currentSong.title} by ${currentSong.description}` : ''} height="200" width="200" loading="lazy" />
            <div class="song-info">
                <h1 class="ds-heading" data-size="xs">{currentSong?.programTitle || 'No program currently playing'}</h1>
                <h1 class="ds-heading" data-size="lg">{currentSong?.title || 'No song is currently playing.'}</h1>
                <p class="ds-paragraph" data-size="md">{(currentSong?.description || '')}</p>
            </div>

            <div class="progress-row">
                <span>{formattedElapsed}</span>
                {#key currentSong?.title}
                    <div id="songLengthProgress">
                        <div
                            id="songLengthProgressBar"
                            style="left: {progressPercent}%"
                        >
                        </div>
                    </div>
                {/key}
                <span>{formattedTotal}</span>
            </div>
        </div>

    </div>

    <div class=content>
        <!-- Denne var ny! ;D -->
        {#each lastFewSongs ?? [] as song}
            <div class="history-player ds-card" data-variant="tinted">
                    <div class="player-top">
                        <img src={song?.imageUrl || fallbackImage} alt={currentSong ? `${currentSong.title} by ${currentSong.description}` : ''} height="200" width="200" loading="lazy" />
                        <div class="song-info">
                            <h1 class="ds-heading" data-size="xs">{song.title}</h1>
                            <p class="ds-paragraph" data-size="sm">{song.description}</p>
                        </div>
                    </div>
            </div>
        {/each}

    </div>
</div>

<style>

.main {
    display: flex;
    flex-direction: column;
    gap: var(--ds-size-6);
    padding: var(--ds-size-6);
}

.main img {
    max-width: 200px;
    max-height: 200px;
}

.content {
    display: flex;
    flex-direction: column;
    gap: var(--ds-size-3);
    margin: 0 0 0 var(--ds-size-14);
}

.history-player img {
    width: 100px;
    height: 100px;
    max-width: 100px;
    max-height: 100px;
}

.history-player .player-top {
    gap: var(--ds-size-6);
}

.history-player .ds-heading {
    margin: 0;
}

.player-top {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: var(--ds-size-15);
}

.progress-row {
  display: flex;
  align-items: center;
  gap: var(--ds-size-4);
  width: 50%;
  margin: 0 auto 0 auto;
}

#songLengthProgress {
    position: relative;
    width: 80%;
    height: 2px;
    background-color: var(--ds-color-neutral-border-subtle);
    margin: 0 auto;
}

#songLengthProgressBar {
    position: absolute;
    top: 50%;
    width: 8px;
    height: 8px;
    border-radius: var(--ds-border-radius-full);
    border: 1px solid var(--ds-color-neutral-border-default);
    background-color: var(--ds-color-neutral-background-default);
    transform: translate(-50%, -50%);
    left: 0%;
    transition: left 1s linear;
}

@keyframes alarm-pulse {
    from {
        transform: scale(1);
        opacity: 1;
    }
    to {
        transform: scale(1.03);
        opacity: 0.85;
    }
}

</style>