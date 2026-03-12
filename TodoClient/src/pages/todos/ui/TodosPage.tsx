import { useState, useEffect } from 'react'
import { Header } from '@/widgets/header'
import { TodoList } from '@/widgets/todo-list'
import { TodoListPanel } from '@/widgets/todo-list-panel'
import { CreateTodoForm } from '@/features/todo'
import { useTodoLists } from '@/entities/todo-list'

export function TodosPage() {
  const [selectedListId, setSelectedListId] = useState<number | null>(null)
  const { data: lists } = useTodoLists()

  // Auto-select first list when lists load
  useEffect(() => {
    if (lists && lists.length > 0 && selectedListId === null) {
      setSelectedListId(lists[0].id)
    }
  }, [lists, selectedListId])

  // If selected list was deleted, reset
  useEffect(() => {
    if (lists && selectedListId !== null) {
      const stillExists = lists.some((l) => l.id === selectedListId)
      if (!stillExists) {
        setSelectedListId(lists.length > 0 ? lists[0].id : null)
      }
    }
  }, [lists, selectedListId])

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      <div className="mx-auto max-w-5xl px-4 py-8">
        <div className="flex gap-6">
          {/* Sidebar */}
          <aside className="w-56 shrink-0">
            <div className="rounded-lg border border-gray-200 bg-white p-4 shadow-sm">
              <TodoListPanel
                selectedListId={selectedListId}
                onSelect={setSelectedListId}
              />
            </div>
          </aside>

          {/* Main content */}
          <main className="flex-1">
            {selectedListId === null ? (
              <div className="flex items-center justify-center h-48 text-gray-400 text-sm">
                Select or create a list to get started
              </div>
            ) : (
              <div className="space-y-6">
                <CreateTodoForm listId={selectedListId} />
                <TodoList listId={selectedListId} />
              </div>
            )}
          </main>
        </div>
      </div>
    </div>
  )
}
