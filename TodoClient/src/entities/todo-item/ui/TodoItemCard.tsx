import { memo } from 'react'
import { formatDate } from '@/shared/lib/formatDate'
import { cn } from '@/shared/lib/cn'
import { TodoItemStatus, getTodoStatusLabel } from '../model/types'
import type { TodoItem } from '../model/types'

interface TodoItemCardProps {
  todo: TodoItem
  actions?: React.ReactNode
}

const statusClasses: Record<TodoItemStatus, string> = {
  [TodoItemStatus.Pending]: 'bg-gray-100 text-gray-600',
  [TodoItemStatus.InProgress]: 'bg-yellow-100 text-yellow-700',
  [TodoItemStatus.Done]: 'bg-green-100 text-green-700',
}

export const TodoItemCard = memo(function TodoItemCard({ todo, actions }: TodoItemCardProps) {
  const isDone = todo.status === TodoItemStatus.Done
  return (
    <div className="flex items-start gap-3 rounded border border-gray-200 bg-white p-3 shadow-sm">
      <div className="flex-1 min-w-0">
        <div className="flex items-center gap-2 flex-wrap">
          <p className={cn('text-sm font-medium', isDone ? 'text-gray-400 line-through' : 'text-gray-800')}>
            {todo.title}
          </p>
          <span className={cn('rounded-full px-2 py-0.5 text-xs font-medium', statusClasses[todo.status])}>
            {getTodoStatusLabel(todo.status)}
          </span>
        </div>
        {todo.notes && <p className="mt-1 text-xs text-gray-500 truncate">{todo.notes}</p>}
        <p className="mt-1 text-xs text-gray-400">{formatDate(todo.createdAt)}</p>
      </div>
      {actions && <div className="flex shrink-0 items-center gap-1">{actions}</div>}
    </div>
  )
})
