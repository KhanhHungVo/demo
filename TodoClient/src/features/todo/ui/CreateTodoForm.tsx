import { useState } from 'react'
import { Button } from '@/shared/ui/Button'
import { Input } from '@/shared/ui/Input'
import { useCreateTodo } from '../api/createTodoMutation'

interface CreateTodoFormProps {
  listId: number
}

export function CreateTodoForm({ listId }: CreateTodoFormProps) {
  const [title, setTitle] = useState('')
  const { mutate, isPending } = useCreateTodo(listId)

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    const trimmed = title.trim()
    if (!trimmed) return
    mutate({ title: trimmed }, { onSuccess: () => setTitle('') })
  }

  return (
    <form onSubmit={handleSubmit} className="flex gap-2">
      <Input
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Add a new todo…"
        disabled={isPending}
      />
      <Button type="submit" disabled={isPending || title.trim().length === 0}>
        {isPending ? 'Adding…' : 'Add'}
      </Button>
    </form>
  )
}
