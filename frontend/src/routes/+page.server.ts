import { PUBLIC_SERVER_ADDR } from '$env/static/public';
import { fail, redirect } from '@sveltejs/kit';
import type { Actions } from './$types';


export const actions: Actions = {
	default: async ({ request }) => {
    console.log("Recieved request to join game")
		const data = await request.formData();
		const room_code = data.get('room_code');
		const username = data.get('username');

		if (room_code === '' || room_code === null || typeof room_code !== 'string') {
			console.error('Missing room code');
			// Return an error
			return fail(400, { room_code, username, missing_room_code: true });
		}

    const server_url = new URL(PUBLIC_SERVER_ADDR);
    server_url.pathname = '/join';
    server_url.searchParams.set('room_code', room_code);

    try{
      const res = await fetch(server_url, {
        method: 'POST',
      })

      const text = await res.text();

      if (!res.ok) {
        return fail(400, { room_code, username, room_code_error: text });
      }
    } catch (err) {
      console.error(err);
    }

		throw redirect(307, `/game/${room_code}/`);
	}
};
