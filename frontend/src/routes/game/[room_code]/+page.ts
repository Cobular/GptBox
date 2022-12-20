import { PUBLIC_SERVER_ADDR } from '$env/static/public';
import { fail } from '@sveltejs/kit';
import type { PageLoad } from './$types';

 
export const load = (async ({ params }) => {
  // Gets the game type for the room code
  const room_code = params.room_code;

  const server_url = new URL(PUBLIC_SERVER_ADDR);
  server_url.pathname = '/type';
  server_url.searchParams.set('room_code', room_code);

  let text;

  try{
    const res = await fetch(server_url, {
      method: 'GET',
    })

    text = await res.text();

    if (!res.ok) {
      return fail(400, { room_code, room_code_error: text });
    }
  } catch (err) {
    console.error(err);
  }

  return {
    game_type: text,
    room_code,
  };
}) satisfies PageLoad;