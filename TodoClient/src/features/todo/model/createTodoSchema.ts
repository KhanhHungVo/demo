import { z } from 'zod'

export const createTodoSchema = z.object({
  title: z.string().min(1, 'Title is required').max(200, 'Title is too long'),
})

export type CreateTodoFormValues = z.infer<typeof createTodoSchema>
