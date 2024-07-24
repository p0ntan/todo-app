<script lang="ts">
	import { page } from '$app/stores';
	import type { SvelteComponent } from 'svelte';
	import type { Todo } from '$lib/types';
	import { onMount } from 'svelte';

	// Stores
	import { getModalStore } from '@skeletonlabs/skeleton';

	// Props
	/** Exposes parent props to this component. */
	export let parent: SvelteComponent;

	const modalStore = getModalStore();

	export let todo: Todo;

	onMount(() => {
		todo.date = todo.date.slice(0, 10);
	});

	// We've created a custom submit function to pass the response and close the modal.
	async function onFormSubmit() {
		try {
			const response = await fetch(`${$page.url.origin}/api-proxy/todos/${todo.id}`, {
				method: 'PUT',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify(todo)
			});

			const result = await response.json();

			if (response.ok) {
				todo = result;
			} else {
				throw new Error('Failed to update todo.');
			}
		} catch (error) {
			console.error('Error:', error);
		}

		if ($modalStore[0].response) $modalStore[0].response(todo);

		modalStore.close();
	}
</script>

<!-- @component This example creates a simple form modal. -->

{#if $modalStore[0]}
	<div class="card p-4 w-modal shadow-xl space-y-4">
		<form class="form space-y-2">
			<input
				class="input h2 rounded-md border border-primary-50"
				type="text"
				bind:value={todo.title}
				placeholder="Enter title..."
			/>
			<textarea
				class="textarea border border-primary-50"
				rows="3"
				bind:value={todo.description}
				placeholder="Enter description."
			/>
			<input class="input rounded-md border border-primary-50" type="date" bind:value={todo.date} />
		</form>

		<footer class="modal-footer {parent.regionFooter}">
			<button class="btn bg-gradient-to-br from-error-50 to-error-200" on:click={parent.onClose}
				>{parent.buttonTextCancel}</button
			>
			<button class="btn bg-gradient-to-br from-success-50 to-success-300" on:click={onFormSubmit}
				>Update todo</button
			>
		</footer>
	</div>
{/if}
