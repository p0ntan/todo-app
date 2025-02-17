import { json } from '@sveltejs/kit';
import { apiFetch } from '$lib/server/apiFetch.js';
import { PUBLIC_REST_API_URL } from '$env/static/public';

export async function PUT({ fetch, cookies, request, params }) {
	const data = await request.json();
	const id = params.id;

	try {
		const response = await apiFetch(fetch, cookies, `${PUBLIC_REST_API_URL}/todos/${id}`, {
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

		return json(result);
	} catch (err) {
		console.error(err);
		return json({ error: 'Unkown server error' }, { status: 500 });
	}
}

export async function DELETE({ fetch, cookies, request, params }) {
	const id = params.id;

	try {
		const response = await apiFetch(fetch, cookies, `${PUBLIC_REST_API_URL}/todos/${id}`, {
			method: 'DELETE',
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
