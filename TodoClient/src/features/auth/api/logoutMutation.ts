import { useMutation } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { useAuthStore } from '@/shared/lib/auth-store'

async function logout(): Promise<void> {
  try {
    await apiClient.post('/api/auth/revoke', {})
  } catch {
    // Ignore errors — still clear local state
  }
}

export function useLogoutMutation() {
  const clearAuth = useAuthStore((s) => s.clearAuth)
  return useMutation({
    mutationFn: logout,
    onSettled: () => {
      clearAuth()
    },
  })
}
