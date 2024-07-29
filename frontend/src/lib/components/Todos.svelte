<script lang="ts">
  	import type { Todo } from '$lib/types';
    import TodoComponent from '$lib/components/Todo.svelte';
    import { createEventDispatcher } from 'svelte';

    export let date = new Date().toISOString().slice(0, 10);
    
    const dispatch = createEventDispatcher();
    let filterDone: boolean = false;
    let filterLeft: boolean = false;

    function updateTodos() {
        todos = todos.filter((t: Todo) => t.date.includes(date));

        dispatch('update', todos);
	}

    function deleteTodo(todo: Todo) {
		todos = todos.filter((t: Todo) => t.id !== todo.id);

        dispatch('update', todos);
	}

    export let todos: Todo[];

    $: doneTodos = todos.filter((t: Todo) => t.finished);
    $: leftTodos = todos.filter((t: Todo) => !t.finished);
    $: filteredTodos = filterDone ? filterLeft ? leftTodos : doneTodos : filterLeft ? leftTodos : todos;
</script>

<div class="flex bg-gray-200 rounded-full mb-6">
        <button on:click={() => {filterDone = false; filterLeft = true}}
            class="btn grow {filterLeft
                ? 'text-black'
                : 'text-gray-500'}"
        >
            <span class="flex flex-col items-center rounded-full">
                Left
            </span>
        </button>
        <button on:click={() => {filterDone = true; filterLeft = false}}
            class="btn grow {filterDone
                ? 'text-black'
                : 'text-gray-500'}"
        >
            <span class="flex flex-col items-center rounded-full">
                Done
            </span>
        </button>
        <button on:click={() => {filterDone = false; filterLeft = false}}
            class="btn grow {(!filterDone && !filterLeft)
                ? 'text-black'
                : 'text-gray-500'}"
        >
            <span class="flex flex-col items-center rounded-full">
                All
            </span>
        </button>
</div>

<ul class="list w-full px-2">
    {#each filteredTodos as todo}
        <TodoComponent {todo} on:update={updateTodos} on:delete={(e) => deleteTodo(e.detail)} />
    {/each}
</ul>
