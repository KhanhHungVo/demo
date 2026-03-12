import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { todoListKeys } from '@/entities/todo-list'

interface CreateTodoListInput {
  title: string
  description?: string
}

async function createTodoList(input: CreateTodoListInput): Promise<{ id: number }> {
  const { data } = await apiClient.post<{ id: number }>('/api/todolists', input)
  return data
}

export function useCreateTodoList() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: createTodoList,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: todoListKeys.all })
    },
  })
}
