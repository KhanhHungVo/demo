import { useState } from 'react'
import { Button } from '@/shared/ui/Button'
import { Input } from '@/shared/ui/Input'
import { useCreateTodoList } from '../api/createTodoListMutation'

interface CreateTodoListFormProps {
  onCreated?: (id: number) => void
}

export function CreateTodoListForm({ onCreated }: CreateTodoListFormProps) {
  const [title, setTitle] = useState('')
  const { mutate, isPending } = useCreateTodoList()

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    const trimmed = title.trim()
    if (!trimmed) return
    mutate({ title: trimmed }, {
      onSuccess: (data) => {
        setTitle('')
        onCreated?.(data.id)
      },
    })
  }

  return (
    <form onSubmit={handleSubmit} className="flex gap-2">
      <Input
        placeholder="New list name…"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        disabled={isPending}
      />
      <Button type="submit" disabled={isPending || !title.trim()}>
        {isPending ? '…' : '+'}
      </Button>
    </form>
  )
}
