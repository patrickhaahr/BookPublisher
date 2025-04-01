import { createFileRoute, useParams } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Calendar, Heart, BookOpen, Star, StarOff, Loader2 } from 'lucide-react'
import { Separator } from '@/components/ui/separator'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { getBookById } from '@/api'
import { 
  getUserBookInteractionByUserAndBook,
  createUserBookInteraction, 
  updateUserBookInteraction 
} from '@/api/userBookInteractions'
import { 
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { getUserIdFromToken } from '@/lib/authUtils'
import type { BookDetails } from '@/types'
import type { UserBookInteraction } from '@/api/userBookInteractions'
import { useEffect } from 'react'

export const Route = createFileRoute('/books/$bookId')({
  component: BookId,
})

function BookId() {
  const { bookId } = useParams({ from: '/books/$bookId' })
  const queryClient = useQueryClient()
  const userId = getUserIdFromToken()

  // Fetch book details
  const { data: book, isLoading: isLoadingBook, error: bookError } = useQuery<BookDetails>({
    queryKey: ['book', bookId],
    queryFn: () => getBookById(bookId),
    staleTime: 1000 * 60 * 5, // Store cached data for 5 minutes
    gcTime: 1000 * 60 * 5, // Remove unused cached data after 5 minutes
  })

  // Fetch user's interaction with this book
  const { 
    data: interaction,
    isLoading: isLoadingInteraction,
    isError: isInteractionError,
    refetch: refetchInteraction
  } = useQuery({
    queryKey: ['bookInteraction', userId, bookId],
    queryFn: async () => {
      if (!userId) return null;
      // Try to get the interaction directly by userId and bookId
      try {
        return await getUserBookInteractionByUserAndBook(userId, bookId);
      } catch (error) {
        console.error("Error fetching interaction:", error);
        return null;
      }
    },
    enabled: !!userId && !!bookId, // Only run if user is logged in and bookId is available
    staleTime: 0, // Always fetch fresh data
  })

  // Create interaction mutation
  const createInteraction = useMutation({
    mutationFn: (newInteraction: Omit<UserBookInteraction, 'interactionId'>) => 
      createUserBookInteraction(newInteraction),
    onSuccess: (data) => {
      // Update the cache with the new interaction data
      queryClient.setQueryData(['bookInteraction', userId, bookId], data)
      // Then refetch to ensure we have the latest data
      refetchInteraction()
    },
    onError: (error) => {
      console.error('Failed to create interaction:', error)
    }
  })

  // Update interaction mutation
  const updateInteraction = useMutation({
    mutationFn: ({ id, data }: { id: string; data: Partial<UserBookInteraction> }) => 
      updateUserBookInteraction(id, data),
    onSuccess: (data) => {
      // Update the cache with the updated interaction data
      queryClient.setQueryData(['bookInteraction', userId, bookId], data)
      // Then refetch to ensure we have the latest data
      refetchInteraction()
    },
    onError: (error) => {
      console.error('Failed to update interaction:', error)
    }
  })

  // Refetch when component mounts or userId changes or bookId changes
  useEffect(() => {
    if (userId && bookId) {
      refetchInteraction()
    }
  }, [userId, bookId, refetchInteraction])

  // Handle interaction updates
  const handleInteractionUpdate = async (data: Partial<UserBookInteraction>) => {
    if (!userId) {
      // Handle not logged in state - maybe show a login dialog
      console.log('User not logged in')
      return
    }

    try {
      if (!interaction) {
        // Create new interaction with only the updated fields
        console.log('Creating new interaction with data:', { 
          userId, 
          bookId, 
          ...data 
        })
        await createInteraction.mutateAsync({
          userId,
          bookId,
          isFavorite: data.isFavorite ?? false,
          isSaved: data.isSaved ?? false,
          status: data.status ?? null,
          rating: data.rating ?? null
        })
      } else {
        // Update existing interaction with only the specified fields
        console.log('Updating existing interaction:', {
          id: interaction.interactionId,
          data
        })
        await updateInteraction.mutateAsync({
          id: interaction.interactionId,
          data
        })
      }
      
      // Force refetch after the operation completes
      await refetchInteraction()
    } catch (error) {
      console.error('Error updating interaction:', error)
    }
  }

  const isUpdating = createInteraction.isPending || updateInteraction.isPending

  if (bookError) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="text-lg text-red-500">Error: {(bookError as Error).message}</div>
      </div>
    )
  }

  if (isLoadingBook || !book) {
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
          {userId && (
            <div className="absolute top-4 right-4 flex gap-2">
              <Button
                variant={interaction?.isFavorite ? "default" : "outline"}
                size="icon"
                className="bg-white/80 backdrop-blur-sm"
                onClick={() => handleInteractionUpdate({ isFavorite: !interaction?.isFavorite })}
                disabled={isUpdating}
              >
                {isUpdating && createInteraction.variables?.isFavorite !== undefined ? (
                  <Loader2 className="h-5 w-5 animate-spin" />
                ) : (
                  <Heart 
                    className={`h-5 w-5 ${interaction?.isFavorite ? "fill-current" : ""} text-red-500`} 
                  />
                )}
              </Button>
            </div>
          )}
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

          {userId && (
            <div className="space-y-4">
              {/* User Interaction Status */}
              {isLoadingInteraction ? (
                <div className="flex items-center justify-center py-2">
                  <Loader2 className="h-5 w-5 animate-spin mr-2" />
                  <span>Loading your data...</span>
                </div>
              ) : isInteractionError ? (
                <div className="text-red-500">Failed to load your interaction data</div>
              ) : (
                <div className="bg-muted/30 p-3 rounded-md">
                  <h3 className="text-sm font-medium mb-2">Your Activity:</h3>
                  <div className="flex flex-wrap gap-x-4 gap-y-1 text-sm">
                    {interaction?.isFavorite && <span className="flex items-center"><Heart className="h-4 w-4 mr-1 fill-red-500 text-red-500" /> Favorited</span>}
                    {interaction?.isSaved && <span>Saved to Library</span>}
                    {interaction?.status && <span>Status: {interaction.status}</span>}
                    {interaction?.rating && <span className="flex items-center">Rated: {interaction.rating}/5 <Star className="h-3 w-3 ml-1 fill-yellow-400 text-yellow-400" /></span>}
                    {!interaction?.isFavorite && !interaction?.isSaved && !interaction?.status && !interaction?.rating && 
                      <span className="text-muted-foreground">No activity yet</span>
                    }
                  </div>
                </div>
              )}

              {/* Rating */}
              <div className="flex items-center gap-2">
                <span className="text-sm font-medium">Rating:</span>
                <div className="flex items-center">
                  {[1, 2, 3, 4, 5].map((rating) => (
                    <Button
                      key={rating}
                      variant="ghost"
                      size="icon"
                      onClick={() => handleInteractionUpdate({ rating })}
                      disabled={isUpdating}
                    >
                      {isUpdating && (createInteraction.variables?.rating === rating || 
                                     updateInteraction.variables?.data?.rating === rating) ? (
                        <Loader2 className="h-5 w-5 animate-spin" />
                      ) : rating <= (interaction?.rating || 0) ? (
                        <Star className="h-5 w-5 fill-yellow-400 text-yellow-400" />
                      ) : (
                        <StarOff className="h-5 w-5 text-gray-300" />
                      )}
                    </Button>
                  ))}
                </div>
              </div>

              {/* Reading Status */}
              <div className="flex items-center gap-2">
                <span className="text-sm font-medium">Status:</span>
                <Select
                  value={interaction?.status || undefined}
                  onValueChange={(value) => handleInteractionUpdate({ status: value })}
                  disabled={isUpdating}
                >
                  <SelectTrigger className="w-[180px]">
                    <SelectValue placeholder="Select status" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Reading">Reading</SelectItem>
                    <SelectItem value="Completed">Completed</SelectItem>
                    <SelectItem value="Want to Read">Want to Read</SelectItem>
                    <SelectItem value="Dropped">Dropped</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <Button 
                  className="w-full"
                  variant={interaction?.status === "Reading" ? "default" : "outline"}
                  onClick={() => handleInteractionUpdate({ status: "Reading" })}
                  disabled={isUpdating}
                >
                  {isUpdating && (createInteraction.variables?.status === "Reading" || 
                    updateInteraction.variables?.data?.status === "Reading") ? (
                    <Loader2 className="mr-2 h-5 w-5 animate-spin" />
                  ) : (
                    <BookOpen className="mr-2 h-5 w-5" />
                  )}
                  Read Now
                </Button>
                <Button 
                  variant={interaction?.isSaved ? "default" : "outline"} 
                  className="w-full"
                  onClick={() => handleInteractionUpdate({ isSaved: !interaction?.isSaved })}
                  disabled={isUpdating}
                >
                  {isUpdating && (createInteraction.variables?.isSaved !== undefined || 
                    updateInteraction.variables?.data?.isSaved !== undefined) ? (
                    <Loader2 className="mr-2 h-5 w-5 animate-spin" />
                  ) : null}
                  {interaction?.isSaved ? 'Saved' : 'Save'}
                </Button>
              </div>
            </div>
          )}

          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <div className="text-3xl font-bold">${book.basePrice.toFixed(2)}</div>
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
