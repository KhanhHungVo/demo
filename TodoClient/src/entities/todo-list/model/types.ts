import type { TodoItem } from '@/entities/todo-item'

export interface TodoList {
  id: number
  title: string
  description?: string
  createdAt: string
  items: TodoItem[]
}
