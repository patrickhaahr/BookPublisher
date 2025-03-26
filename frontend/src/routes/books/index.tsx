import { useQuery } from '@tanstack/react-query'
import { createFileRoute } from '@tanstack/react-router'
import { BookCard, BookCardSkeleton } from '@/components/book-card'
import { useState } from 'react'
import {
  Pagination,
  PaginationContent,
  PaginationItem, 
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
  PaginationEllipsis,
} from "@/components/ui/pagination"

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
  covers: { coverId: string, imgBase64: string }[]
  authors: { authorId: string, firstName: string, lastName: string }[]
}

interface BooksResponse {
  items: Book[]
  total: number
  page: number
  pageSize: number
  totalPages: number
  hasNextPage: boolean
  hasPreviousPage: boolean
}

function Books() {
  const [page, setPage] = useState(1)
  const pageSize = 12
  // TODO: Use useQueries to fetch books and covers in parallel
  const { data : booksData, isPending, error } = useQuery({
    queryKey: ['books', page], // Include page in query key for proper caching
    queryFn: () => getBooks(page, pageSize),
    staleTime: 1000 * 60 * 5, // Store cached data for 5 minutes
    gcTime: 1000 * 60 * 5, // Remove unused cached data after 5 minutes
  })

  if (error) {
    return (
      <div className="text-center text-lg font-semibold text-red-500">
        Error: {error.message}
      </div>
    )
  }

  if (isPending || !booksData?.items.length) {
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

  // Combine books with their covers and authors
  const booksWithCovers = booksData?.items.map((book: Book) => {
    return {
      id: book.slug,
      title: book.title,
      coverUrl: book.covers[0] ? `data:image/png;base64,${book.covers[0].imgBase64}` : '',
      author: book.authors[0] ? `${book.authors[0].firstName} ${book.authors[0].lastName}` : '',
    }
  })

  // Pagination handler
  const handlePageChange = (newPage: number) => {
    if (newPage >= 1 && newPage <= booksData.totalPages) {
      setPage(newPage);
    }
  }

  return (
    <div className="space-y-8">
      <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
        {booksWithCovers.map((book) => (
          <BookCard key={book.id} {...book} />
        ))}
      </div>
      
      {booksData.totalPages > 1 && (
        <Pagination>
          <PaginationContent>
            <PaginationItem>
              <PaginationPrevious 
                href="#" 
                onClick={(e) => {
                  e.preventDefault()
                  handlePageChange(page - 1);
                }}
                className={!booksData.hasPreviousPage ? "pointer-events-none opacity-50" : ""}
              />
            </PaginationItem>
            
            {/* First page */}
            <PaginationItem>
              <PaginationLink 
                href="#" 
                onClick={(e) => {
                  e.preventDefault()
                  handlePageChange(1);
                }}
                isActive={page === 1}
              >
                1
              </PaginationLink>
            </PaginationItem>
            
            {/* Show ellipsis if needed */}
            {page > 3 && (
              <PaginationItem>
                <PaginationEllipsis />
              </PaginationItem>
            )}
            
            {/* Show current page and surrounding pages */}
            {Array.from({ length: 3 }, (_, i) => {
              const pageNum = page - 1 + i
              return pageNum > 1 && pageNum < booksData.totalPages ? (
                <PaginationItem key={pageNum}>
                  <PaginationLink 
                    href="#" 
                    onClick={(e) => {
                      e.preventDefault()
                      handlePageChange(pageNum);
                    }}
                    isActive={page === pageNum}
                  >
                    {pageNum}
                  </PaginationLink>
                </PaginationItem>
              ) : null
            })}
            
            {/* Show ellipsis if needed */}
            {page < booksData.totalPages - 2 && (
              <PaginationItem>
                <PaginationEllipsis />
              </PaginationItem>
            )}
            
            {/* Last page (if more than 1 page exists) */}
            {booksData.totalPages > 1 && (
              <PaginationItem>
                <PaginationLink 
                  href="#" 
                  onClick={(e) => {
                    e.preventDefault()
                    handlePageChange(booksData.totalPages);
                  }}
                  isActive={page === booksData.totalPages}
                >
                  {booksData.totalPages}
                </PaginationLink>
              </PaginationItem>
            )}
            
            <PaginationItem>
              <PaginationNext 
                href="#" 
                onClick={(e) => {
                  e.preventDefault()
                  handlePageChange(page + 1);
                }}
                className={!booksData.hasNextPage ? "pointer-events-none opacity-50" : ""}
              />
            </PaginationItem>
          </PaginationContent>
        </Pagination>
      )}
      
      {/* Show pagination info */}
      {booksData && (
        <div className="text-center text-sm text-muted-foreground">
          Showing {(page - 1) * pageSize + 1} to {Math.min(page * pageSize, booksData.total)} of {booksData.total} books
        </div>
      )}
    </div>
  )
}

const getBooks = async (page: number, pageSize: number) => {
  await new Promise(resolve => setTimeout(resolve, 1000))
  console.log('Fetching books for page:', page)
  const response = await fetch(`http://localhost:5094/api/v1/books?page=${page}&pageSize=${pageSize}`)
  if (!response.ok) throw new Error('Failed to fetch books');
  return await response.json() as BooksResponse;
}