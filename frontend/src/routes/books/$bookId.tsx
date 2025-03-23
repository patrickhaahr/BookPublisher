import { createFileRoute } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Calendar, Heart, BookOpen } from 'lucide-react'
import { Separator } from '@/components/ui/separator'

interface BookDetails {
  bookId: string
  title: string
  publishDate: string
  basePrice: number
  covers: Array<{ url: string }>
  authors: Array<{ name: string, imageUrl: string }>
  mediums: string[]
  genres: string[]
}

export const Route = createFileRoute('/books/$bookId')({
  component: BookId,
  loader: async ({ params }) => {
    await new Promise((resolve) => setTimeout(resolve, 1000))
    // Mock data - replace with actual API call
    return {
      data: {
        bookId: params.bookId,
        title: "The Art of Programming",
        publishDate: "2024-03-15",
        basePrice: 29.99,
        covers: [{ url: "https://picsum.photos/400/600" }],
        authors: [
          { name: "John Doe", imageUrl: "https://picsum.photos/50/50" }
        ],
        mediums: ["Hardcover", "Digital"],
        genres: ["Technology", "Education"]
      } as BookDetails
    }
  },
  pendingComponent: () => <div>Loading...</div>,
  errorComponent: () => <div>Error</div>,
})

function BookId() {
  const { data } = Route.useLoaderData()
  const book = data as BookDetails

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="grid md:grid-cols-2 gap-8">
        {/* Left Column - Book Cover */}
        <div className="relative">
          <img
            src={book.covers[0]?.url}
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
                  <span className="text-sm font-medium">{author.name}</span>
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
