import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
  component: Index,
})

function Index() {
  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Welcome to Book Publisher</h1>
      <p>This is the home page of our application.</p>
    </div>
  )
}