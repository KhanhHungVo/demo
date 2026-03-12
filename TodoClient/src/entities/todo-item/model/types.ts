export const TodoItemStatus = {
  Pending: 0,
  InProgress: 1,
  Done: 2,
} as const

export type TodoItemStatus = (typeof TodoItemStatus)[keyof typeof TodoItemStatus]

export interface TodoItem {
  id: number
  todoListId: number
  title: string
  notes?: string
  status: TodoItemStatus
  dueDate?: string
  createdAt: string
}

export function isTodoItemDone(item: TodoItem): boolean {
  return item.status === TodoItemStatus.Done
}

export function getTodoStatusLabel(status: TodoItemStatus): string {
  switch (status) {
    case TodoItemStatus.Pending: return 'Pending'
    case TodoItemStatus.InProgress: return 'In Progress'
    case TodoItemStatus.Done: return 'Done'
  }
}
