<script lang="ts">
    import { page } from '$app/stores';
    import { createEventDispatcher } from 'svelte';

    const dispatch = createEventDispatcher();

    export let todo: any;

    async function UpdateTodoFinished() {
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

                dispatch('update', todo)
            } else {
                throw new Error('Failed to fetch secret data');
            }
        } catch (error) {
            console.error("Error:", error);
        }
    }
</script>

<li class="border-2 border-primary-500 py-2 px-4 w-full !mb-4 shadow-lg">
    <input class="checkbox rounded-full border-2 border-primary-500" type="checkbox" checked={todo.finished} on:change={UpdateTodoFinished} />
    <p class="{todo.finished ? 'line-through' : ''} text-lg">{todo.title}</p>
</li>
