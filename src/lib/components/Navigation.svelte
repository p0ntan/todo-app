<script lang="ts">
	import { page } from '$app/stores';
	import { goto } from '$app/navigation';
	import Home from '$lib/icons/Home.svelte';
	import Calendar from '$lib/icons/Calendar.svelte';
	import Customize from '$lib/icons/Customize.svelte';

	const paths = [
		{ name: 'Home', path: '/home', icon: Home },
		{ name: 'Calendar', path: '/calendar', icon: Calendar },
		{ name: 'Settings', path: '/settings', icon: Customize }
	];
</script>

<nav
	class="w-full border-t border-surface-800 bg-white p-4"
>
	<div class="flex bg-gray-200 rounded-full">
		{#each paths as { name, path, icon }}
			<button
				class="btn grow text-lg {$page.url.pathname.includes(path) ? 'fill-black' : 'fill-surface-900'}"
				aria-current='page'
				on:click={() => goto(path)}
			>
			<span class="flex flex-col items-center max-h-8 max-w-8 p-2 rounded-full">
				<svelte:component this={icon} />
			</span>
			</button>
		{/each}
	</div>
</nav>

<style>
	button {
		border-radius: 0 !important;
	}
</style>