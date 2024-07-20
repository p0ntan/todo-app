import type { PageServerLoad } from './$types';
import { PRIVATE_GOOGLE_CLIENT_ID, PRIVATE_GOOGLE_CLIENT_SECRET } from '$env/static/private';
import { OAuth2Client } from 'google-auth-library';

export const load: PageServerLoad = async () => {
	const client = new OAuth2Client(
		PRIVATE_GOOGLE_CLIENT_ID,
		PRIVATE_GOOGLE_CLIENT_SECRET,
		"http://localhost:5173/callback"
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
