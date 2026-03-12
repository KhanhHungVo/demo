import { useNavigate } from 'react-router'
import { Button } from '@/shared/ui/Button'
import { useLogoutMutation } from '../api/logoutMutation'

export function LogoutButton() {
  const navigate = useNavigate()
  const { mutate, isPending } = useLogoutMutation()
  return (
    <Button
      variant="ghost"
      onClick={() => mutate(undefined, { onSettled: () => navigate('/login') })}
      disabled={isPending}
    >
      {isPending ? 'Signing out…' : 'Sign out'}
    </Button>
  )
}
