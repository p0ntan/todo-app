import type { PageServerLoad } from "./$types";
import { PUBLIC_REST_API_URL } from "$env/static/public";
import { apiFetch } from "$lib/apiFetch";


export const load: PageServerLoad = async ({ fetch, cookies, locals }) => {
    let todos;

    try {
        const response = await apiFetch(fetch, cookies, `${PUBLIC_REST_API_URL}/todos`, {});
        todos = await response.json()
    } catch (err) {
        console.log(err);
        todos = [];
    }

    return {
        todos
    }
}
