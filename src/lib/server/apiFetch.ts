import { error } from '@sveltejs/kit';
import { PUBLIC_REST_API_URL } from '$env/static/public';
import { setAccessCookie, setRefreshCookie } from '$lib/server/setCookies';

/**
 * Fetches data from the specified URL using the provided fetch function, with optional request options.
 * If the response is not successful (status code other than 200-299), it attempts to refresh the access token
 * and retry the request with the new token. If the refresh token request fails, it throws an error.
 *
 * @param {typeof window.fetch} fetch - The fetch function to use for making the request.
 * @param {any} cookies - The cookies object to set the new access and refresh tokens.
 * @param {string} url - The URL to fetch data from.
 * @param {object} [options={}] - The options for the fetch request.
 * @return {Promise<Response>} A Promise that resolves to the Response object representing the response to the request.
 * @throws {Error} If the refresh token request fails.
 */
export const apiFetch = async (fetch: typeof window.fetch, cookies: any, url: string, options = {}) => {
    let response = await fetch(url, options);

    if (!response.ok) {
        // Refresh access-token
        const refreshResponse = await fetch(`${PUBLIC_REST_API_URL}/auth/refresh-token`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        const refreshData = await refreshResponse.json();

        if (refreshResponse.status !== 200) {
            throw error(refreshResponse.status, refreshData.error);
        }

        setAccessCookie(cookies, refreshData.accessToken);
        setRefreshCookie(cookies, refreshData.refreshToken);

        // Retry the initial request with the new token
        response = await fetch(url, options);
    }

    return response;
};
