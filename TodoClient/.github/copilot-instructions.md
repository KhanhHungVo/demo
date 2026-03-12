# Copilot Instructions — TodoClient

## Commands

```bash
npm run dev          # Start dev server (http://localhost:5173)
npm run build        # Type-check + production build (tsc -b && vite build)
npm run lint         # ESLint all source files
npm run preview      # Preview production build

npx vitest           # Run tests in watch mode
npx vitest run       # Run tests once (CI)
npx vitest run tests/unit/todo-item.test.tsx  # Run a single test file
```

> There is no `npm test` script — use `npx vitest` directly.

---

## Backend API

This app integrates with the **Todo_CA_Auth** .NET API (`../Todo_CA_Auth`).

| Detail | Value |
|---|---|
| Base URL | `http://localhost:5227` (set in `.env` as `VITE_API_BASE_URL`) |
| Auth | JWT Bearer token + httpOnly refresh token cookie |
| Start API | `dotnet run --project src/Todo.API` from `Todo_CA_Auth/` |

### Key Endpoints

| Method | Path | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | public | Register `{ email, password, firstName, lastName }` |
| POST | `/api/auth/login` | public | Login → `{ accessToken, refreshToken, expiresIn }` |
| POST | `/api/auth/refresh` | cookie | Refresh access token using httpOnly cookie |
| POST | `/api/auth/revoke` | cookie | Revoke refresh token (logout) |
| GET | `/api/auth/me` | bearer | Get current user info |
| GET | `/api/todolists` | bearer | Get all todo lists (with items) |
| POST | `/api/todolists` | bearer | Create todo list `{ title, description? }` |
| DELETE | `/api/todolists/{id}` | bearer | Delete todo list |
| POST | `/api/todoitems` | bearer | Create todo item `{ todoListId, title, notes?, dueDate? }` |
| POST | `/api/todoitems/{id}/complete` | bearer | Mark item done |
| POST | `/api/todoitems/{id}/uncomplete` | bearer | Mark item not done |
| DELETE | `/api/todoitems/{id}` | bearer | Delete todo item |

### Error Response Shapes
- **422** validation: `{ errors: [{ propertyName: string, errorMessage: string }] }`
- **401/400/404**: `{ error: string }`

---

## Architecture — Feature-Sliced Design (FSD)

Layers flow **strictly top → bottom**. A layer may only import from layers below it. Cross-slice imports within the same layer are forbidden (ESLint enforced via `eslint-plugin-boundaries`).

```
app       ← providers (React Query, Router, Auth), route definitions, App shell
pages     ← route-level pages, lazy-loaded via React.lazy
widgets   ← composite UI sections; own their Suspense boundaries
features  ← user interactions grouped by domain: auth, todo
entities  ← business models, display-only UI, query hooks
shared    ← API client, auth store, env config, utilities, primitive UI components
```

### Current Slices

**entities/**
- `todo-item` — `TodoItem` type, `TodoItemStatus` enum, `useTodoItems(listId)`, `TodoItemCard`
- `todo-list` — `TodoList` type, `useTodoLists()`, `useTodoList(id)`, `todoListKeys`
- `user` — `CurrentUserDto` type, `useCurrentUser()`

**features/** *(grouped by domain — mutations and UI live together per domain)*
- `auth` — `LoginForm`, `RegisterForm`, `LogoutButton`, `useLoginMutation`, `useRegisterMutation`, `useLogoutMutation`; schemas in `model/`
- `todo` — `CreateTodoForm`, `ToggleTodoButton`, `DeleteTodoButton`, `CreateTodoListForm`, `DeleteTodoListButton`; all mutations in `api/`

**widgets/**
- `header` — `Header` (user greeting + logout button)
- `todo-list` — `TodoList` (assembles `TodoItemCard` + action buttons, takes `listId` prop)
- `todo-list-panel` — `TodoListPanel` (list picker sidebar, create/delete lists, takes `selectedListId` + `onSelect` props)

**pages/**
- `login` — `LoginPage`
- `register` — `RegisterPage`
- `todos` — `TodosPage` (main two-panel layout: list panel + todo area)

**shared/**
- `api/client` — Axios instance with Bearer token request interceptor and 401 auto-refresh response interceptor
- `lib/auth-store` — Zustand store: `accessToken` (memory only), `user`, `setAuth`, `clearAuth`, `isAuthenticated`
- `config/env` — `env.API_BASE_URL`
- `ui/Button`, `ui/Input` — primitive components
- `lib/cn`, `lib/formatDate` — utilities

---

## Auth Architecture

Access token is stored **in memory only** (Zustand, never localStorage) for security. The refresh token is an httpOnly cookie managed by the browser.

```
AuthProvider (app/providers/AuthProvider.tsx)
  └─ on mount: POST /api/auth/refresh → if OK, setAuth(token, user)

apiClient interceptors (shared/api/client.ts)
  ├─ request: attach Authorization: Bearer {token} from auth store
  └─ response: on 401 → POST /api/auth/refresh → retry original request
               on refresh fail → clearAuth() (app redirects to /login)
```

---

## Routes

| Path | Guard | Component |
|---|---|---|
| `/` | — | Redirect → `/lists` |
| `/login` | PublicRoute (redirects if authed) | `LoginPage` |
| `/register` | PublicRoute (redirects if authed) | `RegisterPage` |
| `/lists` | ProtectedRoute (redirects to `/login` if not authed) | `TodosPage` |

`ProtectedRoute` and `PublicRoute` are defined in `src/app/App.tsx` and read `useAuthStore`.

---

## Key Conventions

### Imports
- Use the `@/` alias for all `src/` imports: `import { TodoItemCard } from '@/entities/todo-item'`
- **Always import from a slice's `index.ts` root** — never from internal paths. The ESLint rule `import/no-internal-modules` enforces this.
- Use `import type { ... }` for type-only imports — `verbatimModuleSyntax` is enabled.

### Adding a new feature
Features are grouped by domain. Add new mutations/UI into the appropriate domain folder:
```
src/features/<domain>/        ← "auth" or "todo"
├── api/        ← React Query useMutation definitions
├── model/      ← Zod schemas, TypeScript types
├── ui/         ← React components
└── index.ts    ← Public API — export only what other layers need
```

If a new domain is needed, create a new `features/<domain>/` folder following the same structure.

### Adding a new entity
```
src/entities/<name>/
├── api/        ← useQuery hooks, query key factory
├── model/      ← TypeScript interfaces/types
├── ui/         ← Display-only React components (wrap in React.memo)
└── index.ts    ← Public API
```

### Server state vs client state
- **TanStack Query** — all API data: `useQuery` in `entities/*/api/`, `useMutation` in `features/*/api/`
- **Zustand** — auth session state only (`shared/lib/auth-store`)

### Query key conventions
```ts
// entities/<name>/api/<name>Api.ts
export const todoListKeys = {
  all: ['todo-lists'] as const,
  detail: (id: number) => ['todo-lists', id] as const,
}
```
Mutations invalidate with `queryClient.invalidateQueries({ queryKey: todoListKeys.all })`.

### TodoItem status
`TodoItemStatus` is a numeric enum (0 = Pending, 1 = InProgress, 2 = Done). Use `getTodoStatusLabel(status)` and `isTodoItemDone(item)` helpers from `@/entities/todo-item`. Toggle uses **separate** `/complete` and `/uncomplete` endpoints — not a single toggle.

### Components
- Wrap display-only entity UI in `React.memo()`
- Place `Suspense` boundaries at the **widget** level
- Page components are lazy-loaded via `React.lazy` — wire in `src/pages/index.ts` then `src/app/App.tsx`
- Avoid `&&` with non-boolean left-hand values: use ternary instead

### Styling
- Tailwind CSS v4 via `@tailwindcss/vite` plugin
- Use `cn()` from `@/shared/lib/cn` for conditional class merging
- `Button` variants: `primary` (default), `ghost`, `danger`

### Zod schemas
```ts
// features/<domain>/model/<action>Schema.ts
export const mySchema = z.object({ ... })
export type MyFormValues = z.infer<typeof mySchema>
```
Map API 422 errors (field-level) back to form state using `propertyName` → camelCase field name.

### TypeScript strictness
`strict: true`, `noUnusedLocals`, `noUnusedParameters`, `erasableSyntaxOnly` are all enabled.
Prefer `const` objects or union types over enums (exception: `TodoItemStatus` mirrors the .NET API enum).

### Tests
- Test files live in `tests/unit/`
- `vitest/globals` available (no explicit imports for `describe`, `it`, `expect`)
- `@testing-library/jest-dom` matchers available globally via `tests/setup.ts`
- Wrap query-dependent components in a `QueryClientProvider` test wrapper
