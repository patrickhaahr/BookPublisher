import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/books/$bookId')({
  component: BooksId,
  loader: async ({ params }) => {
    await new Promise((resolve) => setTimeout(resolve, 1000))
    return {
      data: params.bookId,
    }
  },
  pendingComponent: () => <div>Loading...</div>,
  errorComponent: () => <div>Error</div>,
})

function BooksId() {
  const { data } = Route.useLoaderData()

  return <div>Hello "/$booksId"! {data}</div>
}
