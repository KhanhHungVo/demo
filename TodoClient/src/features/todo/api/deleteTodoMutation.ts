import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { todoListKeys } from '@/entities/todo-list'

async function deleteTodo(id: number): Promise<void> {
  await apiClient.delete(`/api/todoitems/${id}`)
}

export function useDeleteTodo() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: deleteTodo,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: todoListKeys.all })
    },
  })
}
