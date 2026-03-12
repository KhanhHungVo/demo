import { z } from 'zod'

export const createTodoListSchema = z.object({
  title: z.string().min(1, 'Title is required').max(100, 'Title too long'),
  description: z.string().max(500, 'Description too long').optional(),
})

export type CreateTodoListFormValues = z.infer<typeof createTodoListSchema>
