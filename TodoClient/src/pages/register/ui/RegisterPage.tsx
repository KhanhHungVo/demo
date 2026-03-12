import { RegisterForm } from '@/features/auth'

export function RegisterPage() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-sm">
        <h1 className="mb-2 text-center text-2xl font-bold text-gray-900">Todo</h1>
        <p className="mb-6 text-center text-sm text-gray-500">Create your account</p>
        <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
          <RegisterForm />
        </div>
      </div>
    </div>
  )
}
