import type { HandleFetch } from '@sveltejs/kit';
import { PUBLIC_REST_API_URL } from '$env/static/public';
import { setAccessCookie, setRefreshCookie } from '$lib/server/setCookies';
import jwt from 'jsonwebtoken';

export async function handle({ event, resolve }) {
    let token: string | undefined = event.cookies.get('access_token');
    const refreshToken: string | undefined = event.cookies.get('refresh_token');

    let user = null;

    if (!token && refreshToken) {
        try {
            const refreshResponse = await fetch(`${PUBLIC_REST_API_URL}/auth/refresh-token`, {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${refreshToken}`
                },
            });
    
            const refreshData = await refreshResponse.json();
    
            if (refreshResponse.ok) {
                token = refreshData.accessToken;

                setAccessCookie(event.cookies, refreshData.accessToken);
                setRefreshCookie(event.cookies, refreshData.refreshToken);

            }
        } catch (err) {
            // do nothing, just catch error
        }
    }

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
	const refreshToken = event.cookies.get("refresh_token");
	const accessToken = event.cookies.get("access_token");

	if (request.url.includes("refresh-token") && refreshToken) {
		request.headers.set('Authorization', `Bearer ${refreshToken}`);
	} else if (request.url.startsWith(PUBLIC_REST_API_URL) && accessToken) {
		request.headers.set('Authorization', `Bearer ${accessToken}`);
	}

	return fetch(request);
};
