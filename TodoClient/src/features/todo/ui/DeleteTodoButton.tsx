import { Button } from '@/shared/ui/Button'
import { useDeleteTodo } from '../api/deleteTodoMutation'

interface DeleteTodoButtonProps {
  todoId: number
}

export function DeleteTodoButton({ todoId }: DeleteTodoButtonProps) {
  const { mutate, isPending } = useDeleteTodo()
  return (
    <Button
      variant="danger"
      onClick={() => mutate(todoId)}
      disabled={isPending}
      aria-label="Delete todo"
    >
      ✕
    </Button>
  )
}
