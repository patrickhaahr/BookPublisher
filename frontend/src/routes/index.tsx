import { createFileRoute } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'

export const Route = createFileRoute('/')({
  component: Index,
})

function Index() {
  return (
    <div className="flex flex-col items-center justify-center min-h-svh">
      <h1 className="text-2xl font-bold mb-4">Welcome to Book Publisher</h1>
      <p>This is the home page of our application.</p>
      <Button>Click me</Button>
    </div>
  )
}