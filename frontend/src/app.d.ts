// See https://kit.svelte.dev/docs/types#app
// for information about these interfaces
// and what to do when importing types
declare namespace App {
	// interface Error {}
	// interface Locals {}
	// interface PageData {}
	// interface Platform {}
}


declare namespace umami {
	function trackEvent(event: string, event_data?: Record<string, unknown>, url?: string, website_id?: string): void;
	function trackView(url: string, referrer?: string, website_id?: string): void;
}