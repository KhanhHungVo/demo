import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import type { CurrentUserDto } from '../model/types'

export const userKeys = {
  current: ['user', 'current'] as const,
}

async function fetchCurrentUser(): Promise<CurrentUserDto> {
  const { data } = await apiClient.get<CurrentUserDto>('/api/auth/me')
  return data
}

export function useCurrentUser(enabled = true) {
  return useQuery({
    queryKey: userKeys.current,
    queryFn: fetchCurrentUser,
    enabled,
    retry: false,
  })
}
