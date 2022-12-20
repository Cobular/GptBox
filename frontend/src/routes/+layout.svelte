<script>
	import '../app.postcss';
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
		</div>
	</div>
</div>

