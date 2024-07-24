import type { PageServerLoad } from './$types';
import { redirect } from '@sveltejs/kit';

export const load: PageServerLoad = async ({ locals }) => {
	if (locals.user)
    {
        throw redirect(302, "/home");
    }

    return {
        user: locals.user
    }
}
