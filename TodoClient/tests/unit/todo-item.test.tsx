import { render, screen } from '@testing-library/react'
import { TodoItemCard } from '../../src/entities/todo-item'
import type { TodoItem } from '../../src/entities/todo-item'

const mockTodo: TodoItem = {
  id: '1',
  title: 'Buy groceries',
  completed: false,
  createdAt: '2024-01-15T10:00:00.000Z',
}

describe('TodoItemCard', () => {
  it('renders the todo title', () => {
    render(<TodoItemCard todo={mockTodo} />)
    expect(screen.getByText('Buy groceries')).toBeInTheDocument()
  })

  it('applies line-through style when completed', () => {
    const completed = { ...mockTodo, completed: true }
    render(<TodoItemCard todo={completed} />)
    const title = screen.getByText('Buy groceries')
    expect(title).toHaveClass('line-through')
  })

  it('renders action slot when provided', () => {
    render(<TodoItemCard todo={mockTodo} actions={<button>Delete</button>} />)
    expect(screen.getByRole('button', { name: 'Delete' })).toBeInTheDocument()
  })
})
