import { useEffect } from 'react'
import { type ReactNode } from 'react'
import axios from 'axios'
import { env } from '@/shared/config/env'
import { useAuthStore } from '@/shared/lib/auth-store'

export function AuthProvider({ children }: { children: ReactNode }) {
  const setAuth = useAuthStore((s) => s.setAuth)

  useEffect(() => {
    async function tryRestoreSession() {
      try {
        const { data: refreshData } = await axios.post(
          `${env.API_BASE_URL}/api/auth/refresh`,
          {},
          { withCredentials: true },
        )
        const { data: user } = await axios.get(`${env.API_BASE_URL}/api/auth/me`, {
          headers: { Authorization: `Bearer ${refreshData.accessToken}` },
          withCredentials: true,
        })
        setAuth(refreshData.accessToken, user)
      } catch {
        // No valid session — stay logged out
      }
    }
    tryRestoreSession()
  }, [setAuth])

  return <>{children}</>
}
