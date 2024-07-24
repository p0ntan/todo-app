<script lang="ts">
	import { page } from '$app/stores';
	import { createEventDispatcher } from 'svelte';
	import type { Todo } from '$lib/types';
	import type { ModalComponent, ModalSettings } from '@skeletonlabs/skeleton';
	import { getModalStore } from '@skeletonlabs/skeleton';
	import TodoDetails from './TodoDetails.svelte';

	const modalStore = getModalStore();
	const dispatch = createEventDispatcher();

	export let todo: Todo;

	async function updateTodoFinished() {
		todo.finished = !todo.finished;

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

				dispatch('update', todo);
			} else {
				todo.finished = !todo.finished;

				throw new Error('Failed to update todo.');
			}
		} catch (error) {
			console.error('Error:', error);
		}
	}

	function openModal() {
		const modalComponent: ModalComponent = {
			ref: TodoDetails,
			props: { todo }
		};
		const modal: ModalSettings = {
			type: 'component',
			component: modalComponent,
			backdropClasses: '!bg-primary-400 !bg-opacity-80',
			response: (response: Todo | false) => {
				if (response) {
					dispatch('update', todo);
				}
			}
		};
		modalStore.trigger(modal);
	}
</script>

<li class="border-2 border-primary-500 py-2 px-4 w-full !mb-4 shadow-lg flex justify-between">
	<div class="flex items-center">
		<input
			class="checkbox rounded-full border-2 border-primary-500 mr-4"
			type="checkbox"
			checked={todo.finished}
			on:change={updateTodoFinished}
		/>
		<p class={todo.finished ? 'line-through' : ''}>{todo.title}</p>
	</div>
	<button
		class="btn btn-icon !bg-gradient-to-br !from-primary-50 !to-primary-400 !text-slate-950 w-8 min-w-8"
		on:click={openModal}
	>
		i
	</button>
</li>
