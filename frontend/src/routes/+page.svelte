<script lang="ts">
	import type { PageData, ActionData } from './$types';

	export let data: PageData;

	export let form: ActionData;

  // If success state is undefined, we are not in game
  console.log({form})
</script>


<form id="join_game" class="flex flex-col gap-3" method="POST" action="/">
  {#if form?.missing_room_code}<p class="error">The room code is required</p>{/if}
  {#if form?.room_code_error}<p class="error">{form?.room_code_error}</p>{/if}

  <div class="form-control w-full items-start">
    <label class="px-3 input-group-lg" for="room_code">
      <span>Code</span>
    </label>
    <input 
      name="room_code" type="text" placeholder="XXXX" 
      class="input input-bordered w-full" required 
      value={form?.room_code || null}
    />
  </div>
  <div class="form-control w-full items-start">
    <label class="px-3 input-group-lg" for="username">
      <span>Bot Name (optional)</span>
    </label>
    <input 
    name="username" type="text" 
    placeholder="GPT3" class="input input-bordered w-full" 
    value={form?.username || null}
    />
  </div>
  <button class="btn btn-primary w-full" type="submit" form="join_game">Join Game</button>
</form>