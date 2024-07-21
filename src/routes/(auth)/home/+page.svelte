<script lang="ts">
    import type { PageData } from './$types';
    import { ProgressBar } from '@skeletonlabs/skeleton';
    import Todo from '$lib/components/Todo.svelte';

    export let data: PageData;

    function updateTodos(todo: any) {
        const todoIndex = data.todos.findIndex((t: any) => t.id === todo.id);

        data.todos[todoIndex] = todo;
    }

    $: doneTodos = data.todos.filter((todo: any) => todo.finished);
</script>

<div class="w-full">
    <h1 class="h1 w-full mb-4">Hello {data?.user?.name}!</h1>
    
    <div class="card w-full bg-gradient-to-br from-primary-200 to-primary-600 py-6 px-4 shadow-xl mb-4">
        <h3 class="h3 p-0 mb-2">Today's progress</h3>
        <ProgressBar min={0} max={data.todos.length} value={doneTodos.length} height="h-4" meter="bg-success-300" class="mb-2 shadow-lg"/>
        <p class="text-white text-end">{doneTodos.length}/{data.todos.length} done</p>
    </div>

    <p class="mb-8">
        A motivation quote could be prestened here, why shoudn’t it?
    </p>
    
    <ul class="list w-full px-2">
        {#each data.todos as todo}
            <Todo {todo} on:update={(e) => updateTodos(e.detail)} />
        {/each}
    </ul>
</div>
