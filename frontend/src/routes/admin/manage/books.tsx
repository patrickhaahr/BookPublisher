import { createFileRoute, redirect, Link } from '@tanstack/react-router'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { checkUserRoleFromToken } from '@/lib/authUtils'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '../../../components/ui/table'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '../../../components/ui/card'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '../../../components/ui/dropdown-menu'
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '../../../components/ui/alert-dialog'
import { Button } from '../../../components/ui/button'
import { Input } from '../../../components/ui/input'
import { Loader2, MoreVertical, Search, Plus, AlertCircle } from 'lucide-react'
import { getBooks } from '../../../api/books'
import type { Book, BooksResponse } from '../../../types/book'

export const Route = createFileRoute('/admin/manage/books')({
  beforeLoad: async ({ location }) => {
    const userRole = checkUserRoleFromToken();
    const isAdmin = userRole === 'Admin';

    if (!isAdmin) {
      throw redirect({
        to: '/auth/login',
        search: {
          redirect: location.pathname + location.search,
        },
      });
    }
  },
  component: RouteComponent,
})

function RouteComponent() {
  const [searchQuery, setSearchQuery] = useState('')
  const [bookToDelete, setBookToDelete] = useState<Book | null>(null)
  const [page, setPage] = useState(1)
  const pageSize = 10
  
  const queryClient = useQueryClient()
  
  const { data: booksResponse, isLoading, isError } = useQuery<BooksResponse>({
    queryKey: ['books', page, pageSize],
    queryFn: async () => {
      return await getBooks(page, pageSize);
    },
    staleTime: 0, // Always consider data stale to force refetch on revisit
    gcTime: 1000 * 60 * 5, // Keep unused data in cache for 5 minutes
    refetchOnMount: true, // Refetch when component mounts
  })
  
  const deleteMutation = useMutation({
    mutationFn: async (bookId: string) => {
      const token = localStorage.getItem('accessToken');
      if (!token) {
        throw new Error('Authentication token not found');
      }
      
      const response = await fetch(`http://localhost:5094/api/v1/books/${bookId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      
      if (!response.ok) {
        throw new Error('Failed to delete book');
      }
      
      return response.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['books'] });
      setBookToDelete(null);
    }
  })
  
  const filteredBooks = booksResponse?.items.filter(book => 
    book.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
    book.slug.toLowerCase().includes(searchQuery.toLowerCase()) ||
    book.bookId.includes(searchQuery)
  )
  
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    })
  }
  
  return (
    <div className="container mx-auto py-6 space-y-6">
      <div className="flex flex-col md:flex-row justify-between gap-4 items-start md:items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Manage Books</h1>
          <p className="text-muted-foreground mt-1">View, edit and manage your published books.</p>
        </div>
        <Button asChild className="shrink-0">
          <Link to="/admin/create/book">
            <Plus className="h-4 w-4 mr-2" />
            Add New Book
          </Link>
        </Button>
      </div>
      
      <Card>
        <CardHeader className="pb-3">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
            <CardTitle>Books Library</CardTitle>
            <div className="relative w-full sm:w-64 md:w-80">
              <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Search books..."
                className="pl-8"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
          </div>
          <CardDescription>
            {filteredBooks ? `${filteredBooks.length} books found` : 'Loading books...'}
          </CardDescription>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="flex justify-center py-8">
              <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
          ) : isError ? (
            <div className="flex flex-col items-center justify-center py-8 text-center">
              <AlertCircle className="h-10 w-10 text-destructive mb-2" />
              <h3 className="font-medium text-lg">Failed to load books</h3>
              <p className="text-muted-foreground max-w-md mt-1">
                There was an error loading your books. Please try again later.
              </p>
              <Button 
                variant="outline" 
                className="mt-4"
                onClick={() => queryClient.invalidateQueries({ queryKey: ['books'] })}
              >
                Retry
              </Button>
            </div>
          ) : (
            <div className="rounded-md border overflow-hidden">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-[80px]">ID</TableHead>
                    <TableHead>Title</TableHead>
                    <TableHead className="hidden md:table-cell">Slug</TableHead>
                    <TableHead className="hidden md:table-cell">Authors</TableHead>
                    <TableHead className="hidden md:table-cell">Published</TableHead>
                    <TableHead className="hidden sm:table-cell">Price</TableHead>
                    <TableHead className="w-[70px]"></TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredBooks && filteredBooks.length > 0 ? (
                    filteredBooks.map((book) => (
                      <TableRow key={book.bookId}>
                        <TableCell className="font-medium">{book.bookId}</TableCell>
                        <TableCell>
                          <div className="font-medium">{book.title}</div>
                          <div className="text-sm text-muted-foreground md:hidden">
                            {book.slug}
                          </div>
                        </TableCell>
                        <TableCell className="hidden md:table-cell">
                          {book.slug}
                        </TableCell>
                        <TableCell className="hidden md:table-cell">
                          {book.authors.map(author => 
                            `${author.firstName} ${author.lastName}`
                          ).join(', ')}
                        </TableCell>
                        <TableCell className="hidden md:table-cell">
                          {formatDate(book.publishDate)}
                        </TableCell>
                        <TableCell className="hidden sm:table-cell">
                          ${book.basePrice.toFixed(2)}
                        </TableCell>
                        <TableCell>
                          <DropdownMenu>
                            <DropdownMenuTrigger asChild>
                              <Button variant="ghost" size="icon">
                                <MoreVertical className="h-4 w-4" />
                                <span className="sr-only">Open menu</span>
                              </Button>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent align="end">
                              <DropdownMenuLabel>Actions</DropdownMenuLabel>
                              <DropdownMenuSeparator />
                              <DropdownMenuItem>
                                <button 
                                  className="w-full text-left" 
                                  onClick={() => window.location.href = `/admin/edit/book/${book.slug}`}
                                >
                                  Edit
                                </button>
                              </DropdownMenuItem>
                              <DropdownMenuItem>
                                <button 
                                  className="w-full text-left" 
                                  onClick={() => window.open(`/books/${book.slug}`, '_blank')}
                                >
                                  View
                                </button>
                              </DropdownMenuItem>
                              <DropdownMenuSeparator />
                              <DropdownMenuItem 
                                className="text-destructive focus:text-destructive"
                                onClick={() => setBookToDelete(book)}
                              >
                                Delete
                              </DropdownMenuItem>
                            </DropdownMenuContent>
                          </DropdownMenu>
                        </TableCell>
                      </TableRow>
                    ))
                  ) : (
                    <TableRow>
                      <TableCell colSpan={8} className="h-24 text-center">
                        No books found.
                      </TableCell>
                    </TableRow>
                  )}
                </TableBody>
              </Table>
            </div>
          )}
          
          {booksResponse && booksResponse.totalPages > 1 && (
            <div className="flex items-center justify-between space-x-2 py-4">
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage(p => Math.max(p - 1, 1))}
                disabled={page === 1 || isLoading}
              >
                Previous
              </Button>
              <div className="text-sm text-muted-foreground">
                Page {page} of {booksResponse.totalPages}
              </div>
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage(p => p + 1)}
                disabled={!booksResponse.hasNextPage || isLoading}
              >
                Next
              </Button>
            </div>
          )}
        </CardContent>
      </Card>

      <AlertDialog open={!!bookToDelete} onOpenChange={(isOpen: boolean) => !isOpen && setBookToDelete(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This will permanently delete{' '}
              <span className="font-medium">{bookToDelete?.title}</span>
              {' '}and all of its content. This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => bookToDelete && deleteMutation.mutate(bookToDelete.bookId)}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
              disabled={deleteMutation.isPending}
            >
              {deleteMutation.isPending ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Deleting...
                </>
              ) : (
                "Delete"
              )}
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  )
}