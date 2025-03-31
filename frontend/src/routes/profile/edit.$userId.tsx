import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/profile/edit/$userId')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/profile/edit/$userId"!</div>
}
