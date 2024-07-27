import { json } from '@sveltejs/kit';
import { apiFetch } from '$lib/server/apiFetch.js';
import { PUBLIC_REST_API_URL } from '$env/static/public';
import { setAccessCookie } from '$lib/server/setCookies.js';

export async function PUT({ fetch, cookies, request, params }) {
	const data = await request.json();
	const id = params.id;

	try {
		const response = await apiFetch(fetch, cookies, `${PUBLIC_REST_API_URL}/users/${id}`, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(data)
		});

		const result = await response.json();

		if (!response.ok) {
			return json(result, { status: response.status });
		}

		setAccessCookie(cookies, result.data.accessToken);

		return json(result);
	} catch (err) {
		console.error(err);
		return json({ error: 'Unkown server error' }, { status: 500 });
	}
}
