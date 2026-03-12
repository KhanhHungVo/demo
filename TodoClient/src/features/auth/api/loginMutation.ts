import { useMutation } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { useAuthStore } from '@/shared/lib/auth-store'

interface LoginInput {
  email: string
  password: string
}

interface LoginResponse {
  accessToken: string
  refreshToken: string
  expiresIn: number
}

async function login(input: LoginInput): Promise<LoginResponse> {
  const { data } = await apiClient.post<LoginResponse>('/api/auth/login', input)
  return data
}

export function useLoginMutation() {
  const setAuth = useAuthStore((s) => s.setAuth)
  return useMutation({
    mutationFn: login,
    onSuccess: async (data) => {
      // Fetch current user info to store in auth state
      const { data: user } = await apiClient.get('/api/auth/me', {
        headers: { Authorization: `Bearer ${data.accessToken}` },
      })
      setAuth(data.accessToken, user)
    },
  })
}
