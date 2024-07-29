import { redirect } from '@sveltejs/kit';
import type { Actions, PageServerLoad } from './$types';

export const load: PageServerLoad = async ({ locals }) => {
	return {
		user: locals.user
	};
};

// Logst out when POST
export const actions: Actions = {
	logout({ cookies }) {
		cookies.delete('access_token', {
			path: '/',
			sameSite: 'lax',
			httpOnly: true,
			secure: true
		});
		cookies.delete('refresh_token', {
			path: '/',
			sameSite: 'lax',
			httpOnly: true,
			secure: true
		});

		redirect(303, '/login');
	}
};
