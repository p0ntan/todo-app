<script lang="ts">
	import { page } from '$app/stores';
	import type { SvelteComponent } from 'svelte';
	import type { Todo } from '$lib/types';
	import { onMount } from 'svelte';
	import SuccessMarker from './Spinner.svelte';

	// Stores
	import { getModalStore } from '@skeletonlabs/skeleton';

	// Props
	/** Exposes parent props to this component. */
	export let parent: SvelteComponent;

	const modalStore = getModalStore();

	export let todo: Todo;

	let isLoading: boolean = false;
	let isDone: boolean = false;
	let edit: boolean = false;

	onMount(() => {
		todo.date = todo.date.slice(0, 10);
	});

	async function deleteTodo() {
		try {
			isLoading = true;

			const response = await fetch(`${$page.url.origin}/api-proxy/todos/${todo.id}`, {
				method: 'DELETE',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify(todo)
			});

			const result = await response.json();

			if (response.ok) {

				isDone = true;

				setTimeout(() => {
					if ($modalStore[0].response) $modalStore[0].response(result);

					modalStore.close();
				}, 750);
			} else {
				throw new Error('Failed to delete todo.');
			}
		} catch (error) {
			console.error('Error:', error);
		}
	}

	async function onFormSubmit() {
		try {
			isLoading = true;

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

				isDone = true;

				setTimeout(() => {
					if ($modalStore[0].response) $modalStore[0].response(todo);

					modalStore.close();
				}, 750);
			} else {
				throw new Error('Failed to update todo.');
			}
		} catch (error) {
			console.error('Error:', error);
		}
	}
</script>

<!-- @component This example creates a simple form modal. -->

{#if $modalStore[0]}
	<div class="card p-4 w-modal shadow-xl space-y-4">
		{#if isLoading}
			<SuccessMarker isChecked={isDone}/>
		{:else}
			{#if edit}
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
			{:else}
				<h2 class="h2 font-light">{todo.title}</h2>
				<p>{todo.description}</p>
				<footer class="modal-footer {parent.regionFooter}">
					<button class="btn bg-gradient-to-br from-success-50 to-success-300" on:click={() => edit = true}
						>Edit</button
					>
					<button class="btn bg-gradient-to-br from-error-50 to-error-200" on:click={deleteTodo}
						>Delete todo</button
					>
				</footer>
			{/if}
		{/if}
	</div>
{/if}
