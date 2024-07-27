<script lang="ts">
	import type { ModalComponent, ModalSettings } from '@skeletonlabs/skeleton';
	import type { Todo } from '$lib/types';
	import { page } from '$app/stores';
	import { onMount } from 'svelte';
	import { getModalStore } from '@skeletonlabs/skeleton';
	import { InlineCalendar } from 'svelte-calendar';
	import TodoCreate from '$lib/components/TodoCreate.svelte';
	import dayjs from 'dayjs';
	import Todos from '$lib/components/Todos.svelte';
	import Heading from '$lib/components/Heading.svelte';

	const modalStore = getModalStore();
	let store: any;
	let todos: Todo[] = [];

	onMount(async () => {
		todos = await getTodos();
	});

	const theme = {
		calendar: {
			width: '700px',
			maxWidth: '86vw',
			legend: {
				height: '2rem'
			},
			colors: {
				text: {
					primary: '#333',
					highlight: '#fff'
				},
				background: {
					primary: '#fff',
					highlight: '#ee6f57',
					hover: '#eee'
				},
				border: '#eee'
			},
			font: {
				regular: '1em',
				large: '37em'
			},
			grid: {
				disabledOpacity: '.35',
				outsiderOpacity: '.6'
			}
		}
	};

	function openCreateModal() {
		const modalComponent: ModalComponent = {
			ref: TodoCreate
		};
		const modal: ModalSettings = {
			type: 'component',
			component: modalComponent,
			backdropClasses: '!bg-primary-400 !bg-opacity-80',
			meta: {date: selectedDate}, 
			response: (response: Todo | false) => {
				if (response) {
					todos = [...todos, response];
				}
			}
		};
		modalStore.trigger(modal);
	}

	async function getTodos() {
		try {
			const response = await fetch(`${$page.url.origin}/api-proxy/todos/date/${selectedDate}`, {
				method: 'GET',
				headers: {
					'Content-Type': 'application/json'
				}
			});

			const result = await response.json();

			if (response.ok) {
				todos = result;

				return result;
			} else {
				throw new Error('Failed to get todos.');
			}
		} catch (error) {
			console.error('Error:', error);
		}
	}

	$: selectedDate = dayjs($store?.selected).format('YYYY-MM-DD');
	$: selectedDate, getTodos();
</script>

<div class="w-full">
	<Heading content="Calendar" />

	<div class="flex justify-center mb-8">
		<div class="rounded-xl overflow-hidden shadow-lg border-2 border-primary-500">
			<InlineCalendar bind:store {theme} />
		</div>
	</div>

	<button
		class="btn bg-gradient-to-br from-primary-400 to-primary-600 text-white mx-auto block mb-6"
		on:click={openCreateModal}>Add todo</button
	>

	<Todos {todos} date={selectedDate} />
</div>
