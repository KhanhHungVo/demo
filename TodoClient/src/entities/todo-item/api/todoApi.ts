import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import type { TodoItem } from '../model/types'

export const todoKeys = {
  all: ['todos'] as const,
  byList: (listId: number) => ['todos', 'list', listId] as const,
}

async function fetchTodoItems(listId: number): Promise<TodoItem[]> {
  const { data } = await apiClient.get<{ items: TodoItem[] }>(`/api/todolists/${listId}`)
  return data.items
}

export function useTodoItems(listId: number | null) {
  return useQuery({
    queryKey: todoKeys.byList(listId!),
    queryFn: () => fetchTodoItems(listId!),
    enabled: listId !== null,
  })
}
