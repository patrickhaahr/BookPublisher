import { useState } from 'react';
import { createFileRoute } from '@tanstack/react-router';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { 
  fetchUserBookInteractions, 
  updateUserBookInteraction, 
  deleteUserBookInteraction,
  type UserBookInteraction 
} from '../../api/userBookInteractions';
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from '../../components/ui/table';
import { 
  Card, 
  CardContent, 
  CardDescription, 
  CardHeader, 
  CardTitle 
} from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { 
  Dialog, 
  DialogContent, 
  DialogDescription, 
  DialogFooter, 
  DialogHeader, 
  DialogTitle 
} from '../../components/ui/dialog';
import { Label } from '../../components/ui/label';
import { Checkbox } from '../../components/ui/checkbox';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../../components/ui/select';
import { AlertCircle, Trash2, Edit, Star } from 'lucide-react';

export const Route = createFileRoute('/admin/manage-interactions')({
  component: ManageInteractionsPage,
});

function ManageInteractionsPage() {
  const queryClient = useQueryClient();
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const [selectedInteraction, setSelectedInteraction] = useState<UserBookInteraction | null>(null);

  // Fetch all user book interactions
  const { data: interactions, isLoading, isError } = useQuery({
    queryKey: ['userBookInteractions'],
    queryFn: fetchUserBookInteractions
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: (id: string) => deleteUserBookInteraction(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['userBookInteractions'] });
      setIsDeleteDialogOpen(false);
    }
  });

  // Update mutation
  const updateMutation = useMutation({
    mutationFn: ({ id, interaction }: { id: string; interaction: Partial<UserBookInteraction> }) => 
      updateUserBookInteraction(id, interaction),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['userBookInteractions'] });
      setIsEditDialogOpen(false);
    }
  });

  const handleDeleteClick = (interaction: UserBookInteraction) => {
    setSelectedInteraction(interaction);
    setIsDeleteDialogOpen(true);
  };

  const handleEditClick = (interaction: UserBookInteraction) => {
    setSelectedInteraction(interaction);
    setIsEditDialogOpen(true);
  };

  const confirmDelete = () => {
    if (selectedInteraction) {
      deleteMutation.mutate(selectedInteraction.interactionId);
    }
  };

  const handleSubmitEdit = (e: React.FormEvent) => {
    e.preventDefault();
    if (selectedInteraction) {
      updateMutation.mutate({
        id: selectedInteraction.interactionId,
        interaction: selectedInteraction
      });
    }
  };

  const handleInteractionChange = (field: keyof UserBookInteraction, value: any) => {
    if (selectedInteraction) {
      setSelectedInteraction({
        ...selectedInteraction,
        [field]: value
      });
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-96">
        <p>Loading...</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="flex items-center justify-center h-96 text-red-500">
        <AlertCircle className="w-6 h-6 mr-2" />
        <p>Error loading user book interactions</p>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6">
      <Card className="shadow-md">
        <CardHeader>
          <CardTitle>User Book Interactions</CardTitle>
          <CardDescription>
            Manage how users are interacting with books in the system
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Username</TableHead>
                <TableHead>Book Title</TableHead>
                <TableHead>Favorite</TableHead>
                <TableHead>Saved</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Rating</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {interactions?.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={7} className="text-center py-6 text-gray-500">
                    No interactions found
                  </TableCell>
                </TableRow>
              ) : (
                interactions?.map((interaction) => (
                  <TableRow key={interaction.interactionId}>
                    <TableCell>{interaction.user?.username ?? 'Unknown'}</TableCell>
                    <TableCell>{interaction.book?.title ?? 'Unknown'}</TableCell>
                    <TableCell>{interaction.isFavorite ? 'Yes' : 'No'}</TableCell>
                    <TableCell>{interaction.isSaved ? 'Yes' : 'No'}</TableCell>
                    <TableCell>{interaction.status ?? 'Not set'}</TableCell>
                    <TableCell>
                      {interaction.rating ? (
                        <div className="flex items-center">
                          <Star className="w-4 h-4 fill-yellow-400 text-yellow-400 mr-1" />
                          <span>{interaction.rating}</span>
                        </div>
                      ) : (
                        'Not rated'
                      )}
                    </TableCell>
                    <TableCell className="text-right">
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => handleEditClick(interaction)}
                        aria-label="Edit interaction"
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => handleDeleteClick(interaction)}
                        aria-label="Delete interaction"
                      >
                        <Trash2 className="w-4 h-4 text-red-500" />
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Delete Confirmation Dialog */}
      <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Confirm Deletion</DialogTitle>
            <DialogDescription>
              Are you sure you want to delete this interaction? This action cannot be undone.
            </DialogDescription>
          </DialogHeader>
          <DialogFooter>
            <Button variant="outline" onClick={() => setIsDeleteDialogOpen(false)}>
              Cancel
            </Button>
            <Button variant="destructive" onClick={confirmDelete} disabled={deleteMutation.isPending}>
              {deleteMutation.isPending ? 'Deleting...' : 'Delete'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Edit Dialog */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Edit Interaction</DialogTitle>
            <DialogDescription>
              Update the interaction details for {selectedInteraction?.user?.username} on "{selectedInteraction?.book?.title}"
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleSubmitEdit}>
            <div className="grid gap-4 py-4">
              <div className="flex items-center gap-4">
                <Label htmlFor="isFavorite" className="w-32">Favorite</Label>
                <Checkbox 
                  id="isFavorite" 
                  checked={selectedInteraction?.isFavorite}
                  onCheckedChange={(checked) => 
                    handleInteractionChange('isFavorite', checked === true)
                  }
                />
              </div>
              <div className="flex items-center gap-4">
                <Label htmlFor="isSaved" className="w-32">Saved</Label>
                <Checkbox 
                  id="isSaved" 
                  checked={selectedInteraction?.isSaved}
                  onCheckedChange={(checked) => 
                    handleInteractionChange('isSaved', checked === true)
                  }
                />
              </div>
              <div className="flex items-center gap-4">
                <Label htmlFor="status" className="w-32">Status</Label>
                <Select 
                  value={selectedInteraction?.status || ''}
                  onValueChange={(value) => handleInteractionChange('status', value)}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select status" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="">Not set</SelectItem>
                    <SelectItem value="Reading">Reading</SelectItem>
                    <SelectItem value="Completed">Completed</SelectItem>
                    <SelectItem value="Want to Read">Want to Read</SelectItem>
                    <SelectItem value="Dropped">Dropped</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="flex items-center gap-4">
                <Label htmlFor="rating" className="w-32">Rating</Label>
                <Select 
                  value={selectedInteraction?.rating?.toString() || ''}
                  onValueChange={(value) => handleInteractionChange('rating', value ? parseInt(value, 10) : null)}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select rating" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="">Not rated</SelectItem>
                    <SelectItem value="1">1 - Poor</SelectItem>
                    <SelectItem value="2">2 - Fair</SelectItem>
                    <SelectItem value="3">3 - Good</SelectItem>
                    <SelectItem value="4">4 - Very Good</SelectItem>
                    <SelectItem value="5">5 - Excellent</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={() => setIsEditDialogOpen(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={updateMutation.isPending}>
                {updateMutation.isPending ? 'Saving...' : 'Save Changes'}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}
