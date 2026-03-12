import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import type { TodoList } from '../model/types'

export const todoListKeys = {
  all: ['todo-lists'] as const,
  detail: (id: number) => ['todo-lists', id] as const,
}

async function fetchTodoLists(): Promise<TodoList[]> {
  const { data } = await apiClient.get<TodoList[]>('/api/todolists')
  return data
}

async function fetchTodoList(id: number): Promise<TodoList> {
  const { data } = await apiClient.get<TodoList>(`/api/todolists/${id}`)
  return data
}

export function useTodoLists() {
  return useQuery({
    queryKey: todoListKeys.all,
    queryFn: fetchTodoLists,
  })
}

export function useTodoList(id: number | null) {
  return useQuery({
    queryKey: todoListKeys.detail(id!),
    queryFn: () => fetchTodoList(id!),
    enabled: id !== null,
  })
}
