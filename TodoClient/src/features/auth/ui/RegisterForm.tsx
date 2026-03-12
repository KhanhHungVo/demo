import { useState } from 'react'
import { useNavigate, Link } from 'react-router'
import { Button } from '@/shared/ui/Button'
import { Input } from '@/shared/ui/Input'
import { registerSchema } from '../model/registerSchema'
import { useRegisterMutation } from '../api/registerMutation'

export function RegisterForm() {
  const navigate = useNavigate()
  const { mutate, isPending } = useRegisterMutation()
  const [values, setValues] = useState({ email: '', password: '', firstName: '', lastName: '' })
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [apiError, setApiError] = useState('')

  function handleChange(field: string, value: string) {
    setValues((prev) => ({ ...prev, [field]: value }))
    setErrors((prev) => ({ ...prev, [field]: '' }))
    setApiError('')
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    const result = registerSchema.safeParse(values)
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
        // Map 422 field errors
        if (err?.response?.status === 422) {
          const fieldErrors: Record<string, string> = {}
          err.response.data.errors?.forEach((e: { propertyName: string; errorMessage: string }) => {
            const field = e.propertyName.charAt(0).toLowerCase() + e.propertyName.slice(1)
            fieldErrors[field] = e.errorMessage
          })
          setErrors(fieldErrors)
        } else {
          setApiError(err?.response?.data?.error ?? 'Registration failed. Please try again.')
        }
      },
    })
  }

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="grid grid-cols-2 gap-3">
        <div>
          <Input
            placeholder="First name"
            value={values.firstName}
            onChange={(e) => handleChange('firstName', e.target.value)}
            disabled={isPending}
          />
          {errors.firstName && <p className="mt-1 text-xs text-red-500">{errors.firstName}</p>}
        </div>
        <div>
          <Input
            placeholder="Last name"
            value={values.lastName}
            onChange={(e) => handleChange('lastName', e.target.value)}
            disabled={isPending}
          />
          {errors.lastName && <p className="mt-1 text-xs text-red-500">{errors.lastName}</p>}
        </div>
      </div>
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
          placeholder="Password (min 8 chars, 1 uppercase, 1 digit)"
          value={values.password}
          onChange={(e) => handleChange('password', e.target.value)}
          disabled={isPending}
        />
        {errors.password && <p className="mt-1 text-xs text-red-500">{errors.password}</p>}
      </div>
      {apiError && <p className="text-sm text-red-500">{apiError}</p>}
      <Button type="submit" disabled={isPending} className="w-full">
        {isPending ? 'Creating account…' : 'Create account'}
      </Button>
      <p className="text-center text-sm text-gray-500">
        Already have an account?{' '}
        <Link to="/login" className="text-blue-600 hover:underline">
          Sign in
        </Link>
      </p>
    </form>
  )
}
