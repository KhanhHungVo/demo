import { Suspense } from 'react'
import { TodoItemCard, useTodoItems } from '@/entities/todo-item'
import { ToggleTodoButton, DeleteTodoButton } from '@/features/todo'

interface TodoListContentProps {
  listId: number
}

function TodoListContent({ listId }: TodoListContentProps) {
  const { data: items, isLoading, isError } = useTodoItems(listId)

  if (isLoading) return <p className="text-center text-sm text-gray-400">Loading…</p>
  if (isError) return <p className="text-center text-sm text-red-500">Failed to load todos.</p>
  if (!items || items.length === 0)
    return <p className="text-center text-sm text-gray-400">No todos yet. Add one above!</p>

  return (
    <ul className="space-y-2">
      {items.map((todo) => (
        <li key={todo.id}>
          <TodoItemCard
            todo={todo}
            actions={
              <>
                <ToggleTodoButton todoId={todo.id} status={todo.status} />
                <DeleteTodoButton todoId={todo.id} />
              </>
            }
          />
        </li>
      ))}
    </ul>
  )
}

interface TodoListProps {
  listId: number
}

export function TodoList({ listId }: TodoListProps) {
  return (
    <Suspense fallback={<p className="text-center text-sm text-gray-400">Loading…</p>}>
      <TodoListContent listId={listId} />
    </Suspense>
  )
}
