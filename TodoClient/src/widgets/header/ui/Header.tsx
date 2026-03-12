import { useAuthStore } from '@/shared/lib/auth-store'
import { LogoutButton } from '@/features/auth'

export function Header() {
  const user = useAuthStore((s) => s.user)
  return (
    <header className="border-b border-gray-200 bg-white px-6 py-4 shadow-sm">
      <div className="flex items-center justify-between">
        <h1 className="text-xl font-bold text-gray-900">Todo</h1>
        {user && (
          <div className="flex items-center gap-4">
            <span className="text-sm text-gray-600">Hello, {user.firstName}</span>
            <LogoutButton />
          </div>
        )}
      </div>
    </header>
  )
}
