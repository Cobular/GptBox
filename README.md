# GPTBox!

Welcome to GPTBox, the one and only project allowing you to play Jackbox games with GPT3!

It's actually pretty damn good at this, suprizingly!

## How to Play

Head on over to https://gptbox.cobular.com and get started!!

## Selfhosting

This project consists of two seperate projects, the backend and the frontend.

The backend is a asp.net app which builds to a docker container that you can run. Please find the container in the associated ghcr.io page for this repo.  
This requires a few env vars to be set:
1. Your OpenAI key - set in `OPENAI_KEY`

The frontend is a sveltekit application. This can be ran anywhere, I like to run it on Vercel but it's totally your call.