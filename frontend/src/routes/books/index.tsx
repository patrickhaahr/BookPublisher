import { createFileRoute } from '@tanstack/react-router'
import { BookCard, BookCardSkeleton } from '@/components/book-card'
import { useQuery } from '@tanstack/react-query'

export const Route = createFileRoute('/books/')({
  component: Books,
})

// Example books data - replace with actual data fetching
const exampleBooks = [
  {
    id: '1',
    title: 'The Great Gatsby',
    author: 'F. Scott Fitzgerald',
    coverUrl: 'https://picsum.photos/300/400?random=1'
  },
  {
    id: '2',
    title: '1984',
    author: 'George Orwell',
    coverUrl: 'https://picsum.photos/300/400?random=2'
  },
  {
    id: '3',
    title: 'Pride and Prejudice',
    author: 'Jane Austen',
    coverUrl: 'https://picsum.photos/300/400?random=3'
  },
  {
    id: '4',
    title: 'To Kill a Mockingbird',
    author: 'Harper Lee',
    coverUrl: 'https://picsum.photos/300/400?random=4'
  },
  {
    id: '5',
    title: 'The Catcher in the Rye',
    author: 'J.D. Salinger',
    coverUrl: 'https://picsum.photos/300/400?random=5'
  },
  {
    id: '6',
    title: 'One Hundred Years of Solitude',
    author: 'Gabriel García Márquez',
    coverUrl: 'https://picsum.photos/300/400?random=6'
  }
]

// Simulate API call
const fetchBooks = async () => {
  // Simulate network delay
  await new Promise(resolve => setTimeout(resolve, 1500))
  return exampleBooks
}

function Books() {
  const { data: books, isLoading } = useQuery({
    queryKey: ['books'],
    queryFn: fetchBooks,
  })

  if (isLoading || !books?.length) {
    return (
      <div className="space-y-8">
        {!books?.length && !isLoading && (
          <p className="text-center text-lg font-semibold text-red-500">Error: Failed to load books</p>
        )}
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
          {Array.from({ length: 6 }).map((_, index) => (
            <BookCardSkeleton key={index} />
          ))}
        </div>
      </div>
    )
  }

  return (
    <div className="space-y-8">
      <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
        {books.map((book) => (
          <BookCard key={book.id} {...book} />
        ))}
      </div>
    </div>
  )
}