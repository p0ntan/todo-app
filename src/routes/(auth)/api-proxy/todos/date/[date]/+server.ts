import { json } from '@sveltejs/kit';
import { apiFetch } from '$lib/server/apiFetch.js';
import { PUBLIC_REST_API_URL } from '$env/static/public';

export async function GET({ fetch, cookies, request, params }) {
	const date = params.date;

	try {
		const response = await apiFetch(fetch, cookies, `${PUBLIC_REST_API_URL}/todos/date/${date}`, {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json'
			}
		});

		const result = await response.json();

		if (!response.ok) {
			return json(result, { status: response.status });
		}

		return json(result);
	} catch (err) {
		console.error(err);
		return json({ error: 'Unkown server error' }, { status: 500 });
	}
}
