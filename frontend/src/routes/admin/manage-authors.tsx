import { createFileRoute, redirect } from '@tanstack/react-router'
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
} from '../../components/ui/table'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '../../components/ui/card'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '../../components/ui/dropdown-menu'
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '../../components/ui/alert-dialog'
import { Button } from '../../components/ui/button'
import { Input } from '../../components/ui/input'
import { Loader2, MoreVertical, Search, AlertCircle } from 'lucide-react'
import { getAuthors, deleteAuthor } from '../../api/authors'
import type { Author, AuthorsResponse } from '../../types/author'

export const Route = createFileRoute('/admin/manage-authors')({
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
  const [authorToDelete, setAuthorToDelete] = useState<Author | null>(null)
  const [page, setPage] = useState(1)
  const pageSize = 10
  
  const queryClient = useQueryClient()
  
  const { data: authorsResponse, isLoading, isError } = useQuery<AuthorsResponse>({
    queryKey: ['authors', page, pageSize, searchQuery],
    queryFn: async () => {
      return await getAuthors(page, pageSize, searchQuery);
    },
    staleTime: 1000 * 60 * 5, // Keep data fresh for 5 minutes
    gcTime: 1000 * 60 * 5, // Keep unused data in cache for 5 minutes
  })
  
  const deleteMutation = useMutation({
    mutationFn: deleteAuthor,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['authors'] });
      setAuthorToDelete(null);
    }
  })
  
  const filteredAuthors = authorsResponse?.items ? authorsResponse.items.filter(author => 
    author.firstName.toLowerCase().includes(searchQuery.toLowerCase()) ||
    author.lastName.toLowerCase().includes(searchQuery.toLowerCase()) ||
    author.email.toLowerCase().includes(searchQuery.toLowerCase()) ||
    author.personId.includes(searchQuery)
  ) : [];
  
  return (
    <div className="container mx-auto py-6 space-y-6">
      <div className="flex flex-col md:flex-row justify-between gap-4 items-start md:items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Manage Authors</h1>
          <p className="text-muted-foreground mt-1">View and manage authors in the system.</p>
        </div>
      </div>
      
      <Card>
        <CardHeader className="pb-3">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
            <CardTitle>Authors</CardTitle>
            <div className="relative w-full sm:w-64 md:w-80">
              <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Search authors..."
                className="pl-8"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
          </div>
          <CardDescription>
            {filteredAuthors ? `${filteredAuthors.length} authors found` : 'Loading authors...'}
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
              <h3 className="font-medium text-lg">Failed to load authors</h3>
              <p className="text-muted-foreground max-w-md mt-1">
                There was an error loading the authors. Please try again later.
              </p>
              <Button 
                variant="outline" 
                className="mt-4"
                onClick={() => queryClient.invalidateQueries({ queryKey: ['authors'] })}
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
                    <TableHead>Name</TableHead>
                    <TableHead className="hidden md:table-cell">Email</TableHead>
                    <TableHead className="hidden sm:table-cell">Royalty Rate</TableHead>
                    <TableHead className="w-[70px]"></TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredAuthors && filteredAuthors.length > 0 ? (
                    filteredAuthors.map((author) => (
                      <TableRow key={author.personId}>
                        <TableCell className="font-medium">{author.personId}</TableCell>
                        <TableCell>
                          <div className="font-medium">
                            {author.firstName} {author.lastName}
                          </div>
                          <div className="text-sm text-muted-foreground md:hidden">
                            {author.email}
                          </div>
                        </TableCell>
                        <TableCell className="hidden md:table-cell">
                          {author.email || 'N/A'}
                        </TableCell>
                        <TableCell className="hidden sm:table-cell">
                          {author.royaltyRate}%
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
                              <DropdownMenuItem 
                                className="text-destructive focus:text-destructive"
                                onClick={() => setAuthorToDelete(author)}
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
                      <TableCell colSpan={5} className="h-24 text-center">
                        No authors found.
                      </TableCell>
                    </TableRow>
                  )}
                </TableBody>
              </Table>
            </div>
          )}
          
          {authorsResponse && authorsResponse.totalPages > 1 && (
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
                Page {page} of {authorsResponse.totalPages}
              </div>
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage(p => p + 1)}
                disabled={!authorsResponse.hasNextPage || isLoading}
              >
                Next
              </Button>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Delete confirmation dialog */}
      <AlertDialog open={!!authorToDelete} onOpenChange={(isOpen: boolean) => !isOpen && setAuthorToDelete(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This will permanently delete the author{' '}
              <span className="font-medium">{authorToDelete?.firstName} {authorToDelete?.lastName}</span>.
              This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => authorToDelete && deleteMutation.mutate(authorToDelete.personId)}
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
