import axios from 'axios'
import { env } from '@/shared/config/env'
import { useAuthStore } from '@/shared/lib/auth-store'

export const apiClient = axios.create({
  baseURL: env.API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true,
})

let isRefreshing = false
let refreshSubscribers: ((token: string) => void)[] = []

function subscribeTokenRefresh(cb: (token: string) => void) {
  refreshSubscribers.push(cb)
}

function onTokenRefreshed(token: string) {
  refreshSubscribers.forEach((cb) => cb(token))
  refreshSubscribers = []
}

apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().accessToken
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`
  }
  return config
})

apiClient.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest = error.config
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve) => {
          subscribeTokenRefresh((token) => {
            originalRequest.headers['Authorization'] = `Bearer ${token}`
            resolve(apiClient(originalRequest))
          })
        })
      }
      originalRequest._retry = true
      isRefreshing = true
      try {
        const { data } = await axios.post(
          `${env.API_BASE_URL}/api/auth/refresh`,
          {},
          { withCredentials: true },
        )
        const newToken: string = data.accessToken
        const { user } = useAuthStore.getState()
        useAuthStore.getState().setAuth(newToken, user!)
        originalRequest.headers['Authorization'] = `Bearer ${newToken}`
        onTokenRefreshed(newToken)
        return apiClient(originalRequest)
      } catch {
        useAuthStore.getState().clearAuth()
        refreshSubscribers = []
        return Promise.reject(error)
      } finally {
        isRefreshing = false
      }
    }
    return Promise.reject(error)
  },
)
