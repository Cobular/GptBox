<script>
	import '../app.postcss';
	import { themeChange } from 'theme-change'
	import { browser } from '$app/environment';
	import Page from './+page.svelte';
	import { onMount } from 'svelte';

	onMount(() => {
		themeChange(false)
	})
</script>

<div class="hero min-h-screen bg-base-200">
	<div class="hero-content text-center">
		<div class="max-w-md">
			<h1 class="text-5xl font-bold">GPTBox</h1>
			<div>
				<p class="pt-6 pb-2">Play Jackbox games with GPT3!</p>
				<p class="pb-6 italic">A real human needs to join the game first!</p>
			</div>

			<div class="card w-96 bg-base-100 shadow-xl">
				<div class="card-body">
						<slot />
					</div>
			</div>
			<button class="pt-6 pb-4" data-toggle-theme="emerald,dracula">change theme</button>
			<p class="pb-2">Made with 🧡 by <a class="link hover:text-secondary transition-colors[" href="https://cobular.com">Cobular</a>, based on work by <a href="https://github.com/tjhorner/JackboxGPT3">TJ Horner</a></p>
		</div>
	</div>
</div>

