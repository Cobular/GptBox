<script lang="ts">
	import { onMount } from 'svelte';
	import type { PageData, ActionData, RouteParams } from './$types';

	export let data: PageData;

	export let form: ActionData;

  // If success state is undefined, we are not in game
  console.log({form})

	const game_type = data?.game_type;
	const room_code = data?.room_code;

  function track_event() {
    umami.trackEvent("start-game", {game_type, room_code})
  }

  onMount(() => {
    track_event()
  })
</script>

<h2 class="text-2xl font-bold">Status: In-game</h2>
<div class="flex flex-col">
  <p>Game: <span class="font-bold">{game_type}</span></p>
  <p>Room: <span class="font-bold">{room_code}</span></p>
  <form method="POST">
    <button class="btn btn-primary mt-3" action="submit">Leave Game</button>
  </form>
</div>