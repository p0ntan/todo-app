import { redirect } from "@sveltejs/kit";
import type { PageServerLoad } from "./$types";
import { PUBLIC_REST_API_URL } from "$env/static/public";
import { apiFetch } from "$lib/server/apiFetch";


export const load: PageServerLoad = async ({ fetch, cookies, locals }) => {
    let todos;
    let date = new Date().toISOString().slice(0, 10);

    if (!locals.user) {
        throw redirect(302, "/login");
    }
}
