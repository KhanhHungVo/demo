import { Button } from '@/shared/ui/Button'
import { useDeleteTodoList } from '../api/deleteTodoListMutation'

interface DeleteTodoListButtonProps {
  listId: number
  onDeleted?: () => void
}

export function DeleteTodoListButton({ listId, onDeleted }: DeleteTodoListButtonProps) {
  const { mutate, isPending } = useDeleteTodoList()
  return (
    <Button
      variant="danger"
      onClick={() => mutate(listId, { onSuccess: onDeleted })}
      disabled={isPending}
      aria-label="Delete list"
    >
      ✕
    </Button>
  )
}
