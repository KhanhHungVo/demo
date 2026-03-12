import { useMutation } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { useAuthStore } from '@/shared/lib/auth-store'

interface RegisterInput {
  email: string
  password: string
  firstName: string
  lastName: string
}

async function register(input: RegisterInput): Promise<void> {
  await apiClient.post('/api/auth/register', input)
  // Auto-login after registration
  const { data: loginData } = await apiClient.post('/api/auth/login', {
    email: input.email,
    password: input.password,
  })
  const { data: user } = await apiClient.get('/api/auth/me', {
    headers: { Authorization: `Bearer ${loginData.accessToken}` },
  })
  useAuthStore.getState().setAuth(loginData.accessToken, user)
}

export function useRegisterMutation() {
  return useMutation({ mutationFn: register })
}
