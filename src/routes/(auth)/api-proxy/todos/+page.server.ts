import type { Actions, PageServerLoad } from "./$types";
import { fail, redirect } from '@sveltejs/kit';

export const load: PageServerLoad = async () => {
	throw redirect(302, '/me');
};

export const actions: Actions = {
    edit: async ({ request, params }) => {
        const data = await request.formData();
        const id = data.get("id");

        console.log("from server", data);

        return {
            thisIsIt: id
        };
    }
}; 
