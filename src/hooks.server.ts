import type { HandleFetch } from '@sveltejs/kit';
import { PUBLIC_REST_API_URL } from '$env/static/public';
import jwt from 'jsonwebtoken';

export async function handle({ event, resolve }) {
    const token: string | undefined = event.cookies.get('access_token');
    
    let user = null;

    if (token) {
        const decoded = jwt.decode(token) as { Name: string; Email: string, UserId: number } | null;
        
        if (decoded) {
            user = {
                name: decoded.Name,
                email: decoded.Email,
                id: Number(decoded.UserId)
            }
        }
    }

    event.locals.user = user;

    return resolve(event);
}

export const handleFetch: HandleFetch = async ({ request, fetch, event }) => {
	const refreshToken = event.cookies.get('refresh_token');
	const accessToken = event.cookies.get('access_token');
	
	if (request.url.includes("refresh-token") && refreshToken) {
		request.headers.set('Authorization', `Bearer ${refreshToken}`);
	} else if (request.url.startsWith(PUBLIC_REST_API_URL) && accessToken) {
		request.headers.set('Authorization', `Bearer ${accessToken}`);
	}

	return fetch(request);
};
