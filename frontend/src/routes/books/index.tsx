import { useQuery } from '@tanstack/react-query'
import { createFileRoute } from '@tanstack/react-router'
import { BookCard, BookCardSkeleton } from '@/components/book-card'

export const Route = createFileRoute('/books/')({
  component: Books,
})

interface Book {
  bookId: string
  title: string
  publishDate: string
  basePrice: number
  slug: string
  mediums: string[]
  genres: string[]
}

interface Cover {
  bookId: string
  imgBase64: string
}

interface BooksResponse {
  books: Book[]
}

function Books() {
  // TODO: Use useQueries to fetch books and covers in parallel
  const { data: booksData, isPending: isBooksLoading, error: booksError } = useQuery({
    queryKey: ['books'],
    queryFn: getBooks,
    staleTime: 1000 * 60 * 5, // Store cached data for 5 minutes
    gcTime: 1000 * 60 * 5, // Remove unused cached data after 5 minutes
  })

  const { data: coversData, isPending: isCoversLoading, error: coversError } = useQuery({
    queryKey: ['covers'],
    queryFn: getCovers,
    staleTime: 1000 * 60 * 5, // Store cached data for 5 minutes
    gcTime: 1000 * 60 * 5, // Remove unused cached data after 5 minutes
  })

  const isLoading = isBooksLoading || isCoversLoading
  const error = booksError || coversError
  const books = booksData?.books
  const covers = coversData as Cover[]

  if (error) {
    return (
      <div className="text-center text-lg font-semibold text-red-500">
        Error: {error.message}
      </div>
    )
  }

  if (isLoading || !books?.length) {
    return (
      <div className="space-y-8">
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
          {Array.from({ length: 6 }).map((_, index) => (
            <BookCardSkeleton key={index} />
          ))}
        </div>
      </div>
    )
  }

  // Combine books with their covers
  const booksWithCovers = books.map(book => {
    const cover = covers?.find(c => c.bookId === book.bookId)
    return {
      id: book.slug,
      title: book.title,
      coverUrl: cover ? `data:image/png;base64,${cover.imgBase64}` : '',
      author: 'Author Name' // We'll need to fetch authors later
    }
  })

  return (
    <div className="space-y-8">
      <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
        {booksWithCovers.map((book) => (
          <BookCard key={book.id} {...book} />
        ))}
      </div>
    </div>
  )
}

const getBooks = async () => {
  await new Promise(resolve => setTimeout(resolve, 1000))
  const response = await fetch('http://localhost:5094/api/v1/books')
  const data = await response.json()
  return data as BooksResponse
}

const getCovers = async () => {
  await new Promise(resolve => setTimeout(resolve, 1000))
  const response = await fetch('http://localhost:5094/api/v1/covers')
  return await response.json() as Cover[]
}