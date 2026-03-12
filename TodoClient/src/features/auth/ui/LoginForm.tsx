import { useState } from 'react'
import { useNavigate, Link } from 'react-router'
import { Button } from '@/shared/ui/Button'
import { Input } from '@/shared/ui/Input'
import { loginSchema } from '../model/loginSchema'
import { useLoginMutation } from '../api/loginMutation'

export function LoginForm() {
  const navigate = useNavigate()
  const { mutate, isPending } = useLoginMutation()
  const [values, setValues] = useState({ email: '', password: '' })
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [apiError, setApiError] = useState('')

  function handleChange(field: string, value: string) {
    setValues((prev) => ({ ...prev, [field]: value }))
    setErrors((prev) => ({ ...prev, [field]: '' }))
    setApiError('')
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    const result = loginSchema.safeParse(values)
    if (!result.success) {
      const fieldErrors: Record<string, string> = {}
      result.error.issues.forEach((issue) => {
        const key = issue.path[0]
        if (key !== undefined) fieldErrors[String(key)] = issue.message
      })
      setErrors(fieldErrors)
      return
    }
    mutate(result.data, {
      onSuccess: () => navigate('/lists'),
      onError: (err: any) => {
        const msg = err?.response?.data?.error ?? 'Login failed. Please try again.'
        setApiError(msg)
      },
    })
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <Input
          type="email"
          placeholder="Email"
          value={values.email}
          onChange={(e) => handleChange('email', e.target.value)}
          disabled={isPending}
        />
        {errors.email && <p className="mt-1 text-xs text-red-500">{errors.email}</p>}
      </div>
      <div>
        <Input
          type="password"
          placeholder="Password"
          value={values.password}
          onChange={(e) => handleChange('password', e.target.value)}
          disabled={isPending}
        />
        {errors.password && <p className="mt-1 text-xs text-red-500">{errors.password}</p>}
      </div>
      {apiError && <p className="text-sm text-red-500">{apiError}</p>}
      <Button type="submit" disabled={isPending} className="w-full">
        {isPending ? 'Signing in…' : 'Sign in'}
      </Button>
      <p className="text-center text-sm text-gray-500">
        No account?{' '}
        <Link to="/register" className="text-blue-600 hover:underline">
          Register
        </Link>
      </p>
    </form>
  )
}
