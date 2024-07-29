import type { Cookies } from '@sveltejs/kit';

export function setAccessCookie(cookies: Cookies, accessToken: string) {
	cookies.set('access_token', accessToken, {
		path: '/',
		sameSite: 'lax',
		httpOnly: true,
		secure: true,
		maxAge: 60 * 15
	});
}

export function setRefreshCookie(cookies: Cookies, refreshToken: string) {
	cookies.set('refresh_token', refreshToken, {
		path: '/',
		sameSite: 'lax',
		httpOnly: true,
		secure: true,
		maxAge: 60 * 60 * 24 * 7
	});
}
