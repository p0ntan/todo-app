<script lang="ts">
	import '../app.postcss';
	import Navigation from '$lib/components/Navigation.svelte';
</script>

<div class="h-screen grid grid-rows-[auto_1fr_auto]">
	<main class="p-4">
		<slot />
	</main>
	<footer class="fixed bottom-0 w-full"><Navigation /></footer>
</div>
