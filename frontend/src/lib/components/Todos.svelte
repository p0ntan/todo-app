<script lang="ts">
  	import type { Todo } from '$lib/types';
    import TodoComponent from '$lib/components/Todo.svelte';
    import { createEventDispatcher } from 'svelte';

    const dispatch = createEventDispatcher();
    export let date = new Date().toISOString().slice(0, 10);

    function updateTodos() {
        todos = todos.filter((t: Todo) => t.date.includes(date));

        dispatch('update', todos);
	}

    function deleteTodo(todo: Todo) {
		todos = todos.filter((t: Todo) => t.id !== todo.id);

        dispatch('update', todos);
	}

    export let todos: Todo[];
</script>

<ul class="list w-full px-2">
    {#each todos as todo}
        <TodoComponent {todo} on:update={updateTodos} on:delete={(e) => deleteTodo(e.detail)} />
    {/each}
</ul>
