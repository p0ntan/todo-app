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
			// FIXME remember to remove this when deploying
			path: '/',
			sameSite: 'lax',
			httpOnly: true,
			secure: false // TODO change this aswell
		});
		cookies.delete('refresh_token', {
			// FIXME remember to remove this when deploying
			path: '/',
			sameSite: 'lax',
			httpOnly: true,
			secure: false // TODO change this aswell
		});

		redirect(303, '/login');
	}
};
