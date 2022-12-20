import { PUBLIC_SERVER_ADDR } from '$env/static/public';
import { redirect } from '@sveltejs/kit';
import type { Actions } from './$types';

export const actions: Actions = {
  default: async ({params}) => {
    const room_code = params.room_code;

    const server_url = new URL(PUBLIC_SERVER_ADDR);
    server_url.pathname = '/disconnect';
    server_url.searchParams.set('room_code', room_code);

    try{
      const status = await fetch(server_url, {
        method: 'GET',
      })
  
      console.log({status});
    } catch (err) {
      console.error(err);
    }

    throw redirect(307, `/`)
  }
};