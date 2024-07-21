<script lang="ts">
	import '../app.postcss';
	import Navigation from '$lib/components/Navigation.svelte';
	import { page } from '$app/stores';

    $: user = $page.data.user;
</script>

<div class="h-screen flex flex-col">
	<main class="p-6 flex-grow flex flex-col items-center justify-evenly">
		<slot />
	</main>

	{#if user}
		<footer class="fixed bottom-0 w-full variant-glass-primary"><Navigation /></footer>
	{/if}
</div>
