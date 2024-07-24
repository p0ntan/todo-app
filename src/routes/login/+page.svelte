<script lang="ts">
	import type { PageData } from './$types';
	import Man from "$lib/icons/Man.svelte";
	import Google from "$lib/icons/Google.svelte";

	export let data: PageData;
</script>

<div class="mt-12">
	<h1 class="h1 text-center mb-2">Welcome onboard!</h1>
	<p class="text-center">All you need to get started is to create your account.</p>
</div>

<div class="flex-grow h-full w-full py-4 flex flex-col items-center overflow-x-hidden">
	<Man />
</div>

<div class="flex w-full mb-6">
	<div class="flex-grow h-1/2 border-b-1 border-primary-500 middle-line"></div>
	<p class="w-fit block mx-2">Continue below</p>
	<div class="flex-grow h-1/2 border-b-1 border-primary-500 middle-line"></div>
</div>

<a type="button" href={data.googleUrl} class="btn btn-lg variant-filled-primary flex justify-between px-4 text-white">
	<span class="bg-white rounded-full p-1 mr-4 -my-2"><Google /></span>
	<span>Sign in with Google</span>
</a>

<style>
	.border-b-1 {
		border-bottom-width: 1px;
	}
</style>
