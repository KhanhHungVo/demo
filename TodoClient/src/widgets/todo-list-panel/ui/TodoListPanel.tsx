import { useTodoLists } from '@/entities/todo-list'
import { CreateTodoListForm, DeleteTodoListButton } from '@/features/todo'
import { cn } from '@/shared/lib/cn'

interface TodoListPanelProps {
  selectedListId: number | null
  onSelect: (id: number) => void
}

export function TodoListPanel({ selectedListId, onSelect }: TodoListPanelProps) {
  const { data: lists, isLoading, isError } = useTodoLists()

  return (
    <div className="flex flex-col gap-3">
      <h2 className="text-sm font-semibold uppercase tracking-wide text-gray-500">My Lists</h2>
      {isLoading && <p className="text-sm text-gray-400">Loading lists…</p>}
      {isError && <p className="text-sm text-red-500">Failed to load lists.</p>}
      {lists && lists.length === 0 && (
        <p className="text-sm text-gray-400">No lists yet. Create one below.</p>
      )}
      {lists && (
        <ul className="space-y-1">
          {lists.map((list) => (
            <li
              key={list.id}
              className={cn(
                'group flex items-center justify-between rounded px-3 py-2 text-sm cursor-pointer',
                selectedListId === list.id
                  ? 'bg-blue-100 text-blue-800 font-medium'
                  : 'text-gray-700 hover:bg-gray-100',
              )}
              onClick={() => onSelect(list.id)}
            >
              <span className="flex-1 truncate">{list.title}</span>
              <span
                className="ml-2 shrink-0 opacity-0 group-hover:opacity-100"
                onClick={(e) => e.stopPropagation()}
              >
                <DeleteTodoListButton listId={list.id} />
              </span>
            </li>
          ))}
        </ul>
      )}
      <CreateTodoListForm onCreated={(id) => onSelect(id)} />
    </div>
  )
}
