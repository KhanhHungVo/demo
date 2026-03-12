import { lazy } from 'react'

export const TodosPage = lazy(() =>
  import('./todos').then((m) => ({ default: m.TodosPage })),
)

export const LoginPage = lazy(() =>
  import('./login').then((m) => ({ default: m.LoginPage })),
)

export const RegisterPage = lazy(() =>
  import('./register').then((m) => ({ default: m.RegisterPage })),
)
