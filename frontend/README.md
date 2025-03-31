# React + TypeScript + Vite

Styling: Tailwind v4, ShadCN UI.

Tanstack Libraries: Query, Router, Form


## Bun commands
bun install
bun run dev
bun pm cache rm

## Tanstack Notes

### Tanstack Query
IsPending - if there is no cache data
IsFetching - if we want to see data, if its cached or not. (everytime we save a file or click refetch it will fetch the data) - whenever the query function is running at all
IsLoading - executing for the first time

// TODO
Implement search functionality (replace current with searchparams?)
ADMIN PANEL: CRUD BOOKS, CRUD AUTHOR, CRUD ARTISTS
    - implement pagination for author and artists

hover over books and it will prefetch - faster experience

login with email / username

Refactor check admin auth into a service?