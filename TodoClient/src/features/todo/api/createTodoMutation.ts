import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { todoListKeys } from '@/entities/todo-list'

interface CreateTodoInput {
  todoListId: number
  title: string
  notes?: string
  dueDate?: string
}

interface CreateTodoResponse {
  id: number
}

async function createTodo(input: CreateTodoInput): Promise<CreateTodoResponse> {
  const { data } = await apiClient.post<CreateTodoResponse>('/api/todoitems', input)
  return data
}

export function useCreateTodo(listId: number) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (input: Omit<CreateTodoInput, 'todoListId'>) =>
      createTodo({ ...input, todoListId: listId }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: todoListKeys.all })
    },
  })
}
