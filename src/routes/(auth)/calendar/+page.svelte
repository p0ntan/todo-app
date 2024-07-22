<script lang="ts">
	import { page } from '$app/stores';
    import { InlineCalendar } from 'svelte-calendar';
    import { onMount } from 'svelte';
    import dayjs from 'dayjs';
	import type { Todo } from '$lib/types';
    import TodoComponent from '$lib/components/Todo.svelte';

    let store: any;
    let todos: Todo[] = [];

    onMount(async () => {
        todos = await getTodos();
    })

    function updateTodos(todo: any) {
        const todoIndex = todos.findIndex((t: any) => t.id === todo.id);

        todos[todoIndex] = todo;
    }

	const theme = {
        "calendar": {
            width: "700px",
            "maxWidth": "86vw",
            "legend": {
                "height": "2rem",
            },
            "colors": {
            "text": {
                "primary": "#333",
                "highlight": "#fff"
            },
            "background": {
                "primary": "#fff",
                "highlight": "#ee6f57",
                "hover": "#eee"
            },
            "border": "#eee"
            },
            "font": {
                "regular": "1em",
                "large": "37em"
            },
            "grid": {
                "disabledOpacity": ".35",
                "outsiderOpacity": ".6"
            }
        }
	};

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
            console.error("Error:", error);
        }
    }

    $: selectedDate = dayjs($store?.selected).format("YYYY-MM-DD");
    $: selectedDate, getTodos();

</script>

<div class="w-full">
    <h1 class="h1 w-full mb-4 mt-12">Calendar</h1>
    
    <div class="flex justify-center mb-8">
        <div class="rounded-xl overflow-hidden shadow-lg border-2 border-primary-500">
            <InlineCalendar bind:store {theme} />
        </div>
    </div>
    
    <ul class="list w-full px-2">
        {#each todos as todo}
            <TodoComponent {todo} on:update={(e) => updateTodos(e.detail)} />
        {/each}
    </ul>

</div>
