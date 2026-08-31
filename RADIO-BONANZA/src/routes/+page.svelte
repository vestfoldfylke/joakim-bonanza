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
        <div class="alarm-banner">🚨 {ALARM_ARTIST.toUpperCase()} SPILLES AV! 🚨</div>
    {/if}

    <div class="player">

        <div class="player-top">
            <img src={currentSong?.imageUrl || fallbackImage} alt={currentSong ? `${currentSong.title} by ${currentSong.description}` : ''} height="200" width="200" loading="lazy" />
            <div class="song-info">
                <h1 style="font-weight:100; font-size:larger; color: rgb(0,0,0, 0.3)">{currentSong?.programTitle || 'No program currently playing'}</h1>
                <h1 style="font-weight:600;">{currentSong?.title || 'No song is currently playing.'}</h1>
                <p style="font-weight:100; font-size: medium;">{(currentSong?.description || '')}</p>
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
            <div class=history-player>
                    <div class="player-top">
                        <img src={song?.imageUrl || fallbackImage} alt={currentSong ? `${currentSong.title} by ${currentSong.description}` : ''} height="200" width="200" loading="lazy" />
                        <div class="song-info">
                            <h1>{song.title}</h1>
                            <p>{song.description}</p>
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
    gap: 20px;
    padding: 20px;
    font-family:'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
}

.main img {
    max-width: 200px;
    max-height: 200px;
}

.content {
    display: flex;
    flex-direction: column;
    gap: 10px;
    margin: 0 0 0 50px;
}

.history-player {
    background-color: rgb(247, 247, 247);
    border-radius: 12px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    padding: 0px;
}

.history-player img {
    width: 100px;
    height: 100px;
    max-width: 100px;
    max-height: 100px;
}

.history-player .player-top {
    gap: 20px;
}

.history-player h1 {
    font-size: 1.2rem;
    margin: 0;
}

.player {
  background-color: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}


.player-top {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 50px;
}

.progress-row {
  display: flex;
  align-items: center;
  gap: 15px;
  width: 50%;
  margin: 0 auto 0 auto;
}

#songLengthProgress {
    position: relative;
    width: 80%;
    height: 2px;
    background-color: rgb(221, 221, 221);
    margin: 0 auto;
}

#songLengthProgressBar {
    position: absolute;
    top: 50%;
    width: 8px;
    height: 8px;
    border-radius: 50%;
    border: 1px solid rgb(126, 126, 126);
    background-color: rgb(255, 255, 255);
    transform: translate(-50%, -50%);
    left: 0%;
    transition: left 1s linear;
}

.alarm-banner {
    background-color: #d90429;
    color: white;
    font-weight: 800;
    font-size: 1.5rem;
    text-align: center;
    padding: 16px;
    border-radius: 12px;
    animation: alarm-pulse 0.6s ease-in-out infinite alternate;
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