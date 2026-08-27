<script lang="ts">
    import { onMount } from 'svelte';
    import { getCurrentSong, type ParsedNrkElement } from '$lib/api/nrk-data.js';

    let progressPercent = $state(0);
    let currentSongTime = $state(0);
    let skipTransition = $state(false);

    let currentSong = $state<ParsedNrkElement>();
    let formattedElapsed = $state('00:00:00');
    let formattedTotal = $state('00:00:00');

    function updateProgressBar(): void {
        if (!currentSong) return;

        const now = Date.now();
        const start = currentSong.startTime.getTime();
        const end = currentSong.songEndTime.getTime();
        progressPercent = Math.min(100, Math.max(0, ((now - start) / (end - start)) * 100));
        currentSongTime = now - start;

        formattedElapsed = currentSongTime ? new Date((Math.floor(currentSongTime / 1000)) * 1000).toISOString().substr(11, 8) : '00:00:00';
        formattedTotal = currentSong ? new Date((Math.floor((currentSong.songEndTime.getTime() - currentSong.startTime.getTime()) / 1000)) * 1000).toISOString().substr(11, 8) : '00:00:00';
    }

    onMount(() => {
        (async () => {
            currentSong = await getCurrentSong();
            updateProgressBar();
        })();

        const interval = setInterval(async () => {
            updateProgressBar();
            const newSong = await getCurrentSong();
            if (newSong?.title !== currentSong?.title) {
                console.log(`Changed from ${currentSong?.title} to ${newSong?.title}`);
                currentSong = newSong;

                updateProgressBar();
                skipTransition = true;
                requestAnimationFrame(() => {
                    skipTransition = false; // slå transition på igjen til neste oppdatering
                });

            }
        }, 1000);
        return () => clearInterval(interval);
    });


</script>

<div class="player" style="visibility: {currentSong ? 'visible' : 'visible'}">

    <div class="player-top">
        <img src={currentSong?.imageUrl} alt={currentSong ? `${currentSong.title} by ${currentSong.description}` : ''} height="200" width="200" loading="lazy" />
        <div class="song-info">
            <h1>{currentSong?.programTitle || 'No program currently playing'}</h1>
            <p><b>{currentSong?.title || 'No song is currently playing.'}</b></p>
            <p>{currentSong?.description || ''}</p>
        </div>

        <div class="progress-row">
            <span>{formattedElapsed}</span>
            <div id="songLengthProgress">
                <div
                    id="songLengthProgressBar"
                    style="left: {progressPercent}%; transition: {skipTransition ? 'none' : 'left 1s linear'}"
                >
                </div>
            </div>
            <span>{formattedTotal}</span>
        </div>
    </div>
    
</div>

<style>

.song-info {
    margin: 0;
}

.player {
  background-color: white;    /* velg selv */
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}


.player-top {
  display: flex;
  flex-direction: row;      /* bilde og tekst side om side */
  align-items: center;      /* midtstiller dem vertikalt mot hverandre */
  gap: 50px;                /* luft mellom bilde og tekst */
}

.progress-row {
  display: flex;
  align-items: center;
  width: 50%;        /* flytt bredden hit fra #songLengthProgress */
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
}

</style>