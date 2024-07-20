<script lang="ts">
	import { page } from '$app/stores';
	import { goto } from '$app/navigation';

	const paths = [
		{ name: 'Home', path: '/home' },
		{ name: 'Calendar', path: '/calendar' },
		{ name: 'Settings', path: '/settings' }
	];
</script>

<nav
	class="flex w-full border-t border-surface-800 text-surface-300"
>
	{#each paths as { name, path }}
		<button
			class="btn grow text-lg {$page.url.pathname.includes(path) ? 'text-black' : 'text-surface-500'}"
			aria-current='page'
			on:click={() => goto(path)}
		>
		<span class="flex flex-col items-center">
			{name}
		</span>
		</button>
	{/each}
</nav>

<style>
	button {
		border-radius: 0 !important;
	}
</style>