import { lazy, Suspense } from 'react'
import { Routes, Route, Navigate, useLocation } from 'react-router'
import { QueryProvider } from './providers/QueryProvider'
import { RouterProvider } from './providers/RouterProvider'
import { AuthProvider } from './providers/AuthProvider'
import { useAuthStore } from '@/shared/lib/auth-store'
import './styles/globals.css'

const TodosPage = lazy(() =>
  import('@/pages/todos').then((m) => ({ default: m.TodosPage })),
)
const LoginPage = lazy(() =>
  import('@/pages/login').then((m) => ({ default: m.LoginPage })),
)
const RegisterPage = lazy(() =>
  import('@/pages/register').then((m) => ({ default: m.RegisterPage })),
)

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated())
  const location = useLocation()
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />
  }
  return <>{children}</>
}

function PublicRoute({ children }: { children: React.ReactNode }) {
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated())
  if (isAuthenticated) {
    return <Navigate to="/lists" replace />
  }
  return <>{children}</>
}

export function App() {
  return (
    <QueryProvider>
      <RouterProvider>
        <AuthProvider>
          <Suspense fallback={null}>
            <Routes>
              <Route path="/login" element={<PublicRoute><LoginPage /></PublicRoute>} />
              <Route path="/register" element={<PublicRoute><RegisterPage /></PublicRoute>} />
              <Route path="/lists" element={<ProtectedRoute><TodosPage /></ProtectedRoute>} />
              <Route path="/" element={<Navigate to="/lists" replace />} />
            </Routes>
          </Suspense>
        </AuthProvider>
      </RouterProvider>
    </QueryProvider>
  )
}
