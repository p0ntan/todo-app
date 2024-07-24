<script lang="ts">
	import '../app.postcss';
	import Navigation from '$lib/components/Navigation.svelte';
	import { page } from '$app/stores';
	import { initializeStores, Modal } from '@skeletonlabs/skeleton';

	initializeStores();

	$: user = $page.data.user;
</script>

<div class="h-screen flex flex-col">
	<main class="p-6 overflow-y-auto flex-grow flex flex-col items-center justify-between">
		<slot />
	</main>

	{#if user}
		<footer class="w-full variant-glass-primary"><Navigation /></footer>
	{/if}
</div>

<Modal />
