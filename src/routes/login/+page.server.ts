import { redirect } from '@sveltejs/kit';
import type { PageServerLoad } from './$types';
import { PRIVATE_GOOGLE_CLIENT_ID, PRIVATE_GOOGLE_CLIENT_SECRET, PRIVATE_CALLBACK_URL } from '$env/static/private';
import { OAuth2Client } from 'google-auth-library';

export const load: PageServerLoad = async ({ locals }) => {
	if (locals.user)
	{
		throw redirect(302, "/home");
	}

	const client = new OAuth2Client(
		PRIVATE_GOOGLE_CLIENT_ID,
		PRIVATE_GOOGLE_CLIENT_SECRET,
		PRIVATE_CALLBACK_URL
	);

	const scopes = [
		"https://www.googleapis.com/auth/userinfo.email",
		"https://www.googleapis.com/auth/userinfo.profile",
	];
	const url = client.generateAuthUrl({
		// access_type: 'offline',
		scope: scopes,
	});

	return {
		googleUrl: url,
	};
};
