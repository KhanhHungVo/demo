import { Button } from '@/shared/ui/Button'
import { TodoItemStatus } from '@/entities/todo-item'
import { useToggleTodo } from '../api/toggleTodoMutation'

interface ToggleTodoButtonProps {
  todoId: number
  status: TodoItemStatus
}

export function ToggleTodoButton({ todoId, status }: ToggleTodoButtonProps) {
  const { mutate, isPending } = useToggleTodo()
  const isDone = status === TodoItemStatus.Done
  return (
    <Button
      variant="ghost"
      onClick={() => mutate({ id: todoId, currentStatus: status })}
      disabled={isPending}
      aria-label={isDone ? 'Mark incomplete' : 'Mark complete'}
    >
      {isDone ? '↩' : '✓'}
    </Button>
  )
}
