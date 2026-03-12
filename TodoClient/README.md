# TodoClient

A React + TypeScript todo application using **Feature-Sliced Design (FSD)**, integrated with the [Todo_CA_Auth](../Todo_CA_Auth) .NET Clean Architecture API.

## Stack

| Concern | Library |
|---|---|
| UI | React 19 + TypeScript |
| Build | Vite |
| Routing | React Router v7 |
| Server state | TanStack Query v5 |
| Client state | Zustand v5 (auth session) |
| Validation | Zod |
| HTTP | Axios (Bearer token + auto-refresh) |
| Styling | Tailwind CSS v4 |
| Tests | Vitest + Testing Library |

## Prerequisites

Start the .NET API first:
```bash
cd ../Todo_CA_Auth
dotnet run --project src/Todo.API
# Runs on http://localhost:5227
```

## Getting Started

```bash
npm install
npm run dev     # http://localhost:5173
```

The `.env` file is pre-configured to point at `http://localhost:5227`.

## Auth Flow

1. **Register** at `/register` — creates account and auto-logs in
2. **Login** at `/login` — receives a short-lived JWT (15 min) stored in memory
3. **Refresh** — the API sets an httpOnly cookie; on page reload `AuthProvider` calls `/api/auth/refresh` automatically to restore the session
4. **Logout** — revokes the refresh token cookie and clears in-memory state

## Architecture

Feature-Sliced Design — layers import strictly **top → bottom**, enforced by ESLint `eslint-plugin-boundaries`.

```
src/
├── app/        ← providers (Query, Router, Auth), routes, global styles
├── pages/      ← route-level pages, lazy-loaded (login, register, todos/lists)
├── widgets/    ← composite UI sections (Header, TodoList, TodoListPanel)
├── features/   ← user interactions grouped by domain (auth, todo)
├── entities/   ← business models + query hooks (todo-item, todo-list, user)
└── shared/     ← API client, auth store, env config, primitive UI
```

### Features (grouped by domain)

```
features/
├── auth/       ← login, register, logout (forms, mutations, schemas)
└── todo/       ← CRUD for todo items and todo lists (mutations, forms, buttons)
```

## Routes

| Path | Access | Description |
|---|---|---|
| `/` | public | Redirects to `/lists` |
| `/login` | public | Login page (redirects to `/lists` if already authenticated) |
| `/register` | public | Register page |
| `/lists` | protected | Main app — list picker + todo items |

## Scripts

| Command | Description |
|---|---|
| `npm run dev` | Start dev server |
| `npm run build` | Type-check + production build |
| `npm run lint` | Lint source files |
| `npx vitest run` | Run unit tests once |
