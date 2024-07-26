<script lang="ts">
	import { page } from '$app/stores';
	import { ProgressBar } from '@skeletonlabs/skeleton';
	import type { Todo } from '$lib/types';
	import type { ModalComponent, ModalSettings } from '@skeletonlabs/skeleton';
	import { getModalStore } from '@skeletonlabs/skeleton';
	import TodoCreate from '$lib/components/TodoCreate.svelte';
	import Todos from '$lib/components/Todos.svelte';

	const modalStore = getModalStore();

	let todos: Todo[] = $page.data.todos;

	function openCreateModal() {
		const modalComponent: ModalComponent = {
			ref: TodoCreate
		};
		const modal: ModalSettings = {
			type: 'component',
			component: modalComponent,
			backdropClasses: '!bg-primary-400 !bg-opacity-80',
			response: (response: Todo | false) => {
				if (response) {
					todos = [...todos, response];
				}
			}
		};
		modalStore.trigger(modal);
	}

	$: doneTodos = todos.filter((todo: Todo) => todo.finished);
</script>

<div class="w-full">
	<h1 class="h1 w-full mb-4 mt-12">Hello {$page.data.user.name}!</h1>

	<div
		class="card w-full bg-gradient-to-br from-primary-200 to-primary-600 py-6 px-4 shadow-xl mb-4"
	>
		<h3 class="h3 p-0 mb-2">Today's progress</h3>
		<ProgressBar
			min={0}
			max={todos.length}
			value={doneTodos.length}
			height="h-4"
			meter="bg-success-300"
			class="mb-2 shadow-lg"
		/>
		<p class="text-white text-end">{doneTodos.length}/{todos.length} done</p>
	</div>

	<button
		class="btn bg-gradient-to-br from-primary-400 to-primary-600 text-white mx-auto block mb-6"
		on:click={openCreateModal}>Add todo</button
	>

	<Todos {todos} on:update={(e) => todos = e.detail} />
</div>
