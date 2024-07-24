<script lang="ts">
	import { page } from "$app/stores";
    import { enhance } from "$app/forms";

    const user = $page.data.user

    let firstName = user?.name;

    async function updateUser() {
        try {
            const response = await fetch(`${$page.url.origin}/api-proxy/users/${user.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ firstName })
            });

            if (!response.ok) {
                throw new Error('Failed to update user.');
            }
        } catch (error) {
            console.error("Error:", error);
        }
    }
</script>

<div class="w-full h-full flex flex-col justify-between">
    <div>
        <h1 class="h1 w-full mb-4 mt-12">Settings</h1>
    
        <div>
            <label class="label">
                <span>Name:</span>
                <input type="text" name="firstname" class="input rounded-md" bind:value={firstName}>
            </label>

            <button class="btn bg-gradient-to-br from-success-200 to-success-300 mx-auto block mb-6" on:click={updateUser}>Save</button>
        </div>
    </div>

    <form method="post" action="/settings?/logout" class="justify-end" use:enhance>
        <button class="btn bg-gradient-to-br from-error-400 to-error-500 text-white mx-auto block mb-6">Log out</button>
    </form>
</div>
