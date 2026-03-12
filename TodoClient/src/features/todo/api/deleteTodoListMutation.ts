import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { todoListKeys } from '@/entities/todo-list'

async function deleteTodoList(id: number): Promise<void> {
  await apiClient.delete(`/api/todolists/${id}`)
}

export function useDeleteTodoList() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: deleteTodoList,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: todoListKeys.all })
    },
  })
}
