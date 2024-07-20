import type { PageServerLoad } from './$types';
import { redirect, error } from '@sveltejs/kit';
import { PRIVATE_GOOGLE_CLIENT_ID, PRIVATE_GOOGLE_CLIENT_SECRET } from '$env/static/private';
import { PUBLIC_REST_API_URL } from '$env/static/public';
import { OAuth2Client } from 'google-auth-library';

export const load: PageServerLoad = async ({ url, cookies }) => {
	const authCode = url.searchParams.get('code');

    const client = new OAuth2Client(
		PRIVATE_GOOGLE_CLIENT_ID,
		PRIVATE_GOOGLE_CLIENT_SECRET,
		"http://localhost:5173/callback"
	);

    if (typeof authCode !== 'string' || !authCode) {
		const errorType = url.searchParams.get('error');
		const errorDescription = url.searchParams.get('error_description');

		if (typeof errorType === 'string' && typeof errorDescription === 'string') {
			console.error(`Error authenticating with Google: ${errorType} (${errorDescription})`);

			error(
				403,
				'Inloggning misslyckades. För att logga in måste du godkänna den här webbplatsen.'
			);
		}

		throw redirect(304, '/');
	}

    const { tokens } = await client.getToken(authCode);

    if (!tokens) {
        throw redirect(304, '/');
    }

    const loginResponseAPI = await fetch(`${PUBLIC_REST_API_URL}/auth`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${tokens.access_token}`,
        },
    });

	const parsedResponse = await loginResponseAPI.json();

    if (!loginResponseAPI.ok) {
		throw redirect(304, '/');
    }

    console.log(parsedResponse);

    cookies.set('access_token', parsedResponse.accessToken, {
		path: '/',
		sameSite: 'strict',
        httpOnly: true,
		secure: false, // TODO change this aswell
		maxAge: 60 * 15
	});

    cookies.set('refresh_token', parsedResponse.refreshToken, {
		path: '/',
		sameSite: 'strict',
        httpOnly: true,
		secure: false, // TODO change this aswell
		maxAge: 60 * 60 * 24 * 7
	});

    throw redirect(302, "/home");
}
