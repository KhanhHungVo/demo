import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/shared/api/client'
import { todoListKeys } from '@/entities/todo-list'
import { TodoItemStatus } from '@/entities/todo-item'

interface ToggleTodoInput {
  id: number
  currentStatus: TodoItemStatus
}

async function toggleTodo({ id, currentStatus }: ToggleTodoInput): Promise<void> {
  const action = currentStatus === TodoItemStatus.Done ? 'uncomplete' : 'complete'
  await apiClient.post(`/api/todoitems/${id}/${action}`)
}

export function useToggleTodo() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: toggleTodo,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: todoListKeys.all })
    },
  })
}
