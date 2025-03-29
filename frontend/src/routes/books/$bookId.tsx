import { createFileRoute, useParams } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Calendar, Heart, BookOpen } from 'lucide-react'
import { Separator } from '@/components/ui/separator'
import { useQuery } from '@tanstack/react-query'
import { getBookById } from '@/api'
import type { BookDetails } from '@/types'

export const Route = createFileRoute('/books/$bookId')({
  component: BookId,
})

function BookId() {
  const { bookId } = useParams({ from: '/books/$bookId' })
  const { data: book, isLoading, error } = useQuery<BookDetails>({
    queryKey: ['book', bookId],
    queryFn: () => getBookById(bookId),
    staleTime: 1000 * 60 * 5, // Store cached data for 5 minutes
    gcTime: 1000 * 60 * 5, // Remove unused cached data after 5 minutes
  })

  if (error) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="text-lg text-red-500">Error: {(error as Error).message}</div>
      </div>
    )
  }

  if (isLoading || !book) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="text-lg">Loading book details...</div>
      </div>
    )
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="grid md:grid-cols-2 gap-8">
        {/* Left Column - Book Cover */}
        <div className="relative">
          <img
            src={book.covers[0]?.imgBase64 ? `data:image/jpeg;base64,${book.covers[0].imgBase64}` : ''}
            alt={book.title}
            className="w-full rounded-lg shadow-2xl object-cover aspect-[1/1]"
          />
          <div className="absolute top-4 right-4">
            <Button variant="outline" size="icon" className="bg-white/80 backdrop-blur-sm">
              <Heart className="h-5 w-5 text-red-500" />
            </Button>
          </div>
        </div>

        {/* Right Column - Book Details */}
        <div className="space-y-6">
          <div>
            <h1 className="text-4xl font-bold tracking-tight">{book.title}</h1>
            <div className="flex items-center gap-4 mt-4">
              {book.authors.map((author, index) => (
                <div key={index} className="flex items-center gap-2">
                  <span className="text-sm font-medium">{`${author.firstName} ${author.lastName}`}</span>
                </div>
              ))}
              {book.covers[0]?.artists.map((artist, index) => (
                <div key={index} className="flex items-center gap-2">
                  <span className="text-sm text-muted-foreground">
                    Cover by {`${artist.firstName} ${artist.lastName}`}
                  </span>
                </div>
              ))}
            </div>
          </div>

          <div className="flex items-center gap-2 text-muted-foreground">
            <Calendar className="h-4 w-4" />
            <span className="text-sm">Published {new Date(book.publishDate).toLocaleDateString()}</span>
          </div>

          <div className="flex flex-wrap gap-2">
            {book.genres.map((genre, index) => (
              <Badge key={index} variant="secondary">{genre}</Badge>
            ))}
          </div>

          <Separator />

          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <div className="text-3xl font-bold">${book.basePrice.toFixed(2)}</div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <Button className="w-full">
                <BookOpen className="mr-2 h-5 w-5" /> Read Now
              </Button>
              <Button variant="outline" className="w-full">Save</Button>
            </div>
          </div>

          <Card className="p-4 bg-muted/50">
            <div className="flex flex-wrap gap-2">
              <span className="text-sm font-medium">Mediums:</span>
              {book.mediums.map((medium, index) => (
                <Badge key={index} variant="outline">{medium}</Badge>
              ))}
            </div>
          </Card>
        </div>
      </div>
    </div>
  )
}
