import { createFileRoute } from '@tanstack/react-router'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { useState, useEffect, useRef } from 'react'
import { useForm } from '@tanstack/react-form'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '../../components/ui/card'
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from '../../components/ui/tabs'
import { Button } from '../../components/ui/button'
import { Input } from '../../components/ui/input'
import { Label } from '../../components/ui/label'
import { Checkbox } from '../../components/ui/checkbox'
import { getBookById, fileToBase64, updateBook } from '../../api/books'
import { BookDetails, EditBookFormValues } from '../../types/book'
import { 
  ArrowLeft, 
  Loader2, 
  Save, 
  AlertCircle, 
  Image as ImageIcon,
  BookOpen,
  UploadIcon
} from 'lucide-react'
import { Link, useNavigate } from '@tanstack/react-router'
import { type CheckedState } from "@radix-ui/react-checkbox"
import { ScrollArea } from '../../components/ui/scroll-area'
import { GENRES, MEDIUMS } from '../../constants/bookOptions'

export const Route = createFileRoute('/admin/edit-book/$bookId')({
  component: EditBookPage,
})

function EditBookPage() {
  const { bookId } = Route.useParams()
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [coverPreview, setCoverPreview] = useState<string | null>(null)
  const [activeTab, setActiveTab] = useState('details')
  const [fileName, setFileName] = useState<string | null>(null)
  const fileInputRef = useRef<HTMLInputElement>(null)

  // Fetch book data
  const { 
    data: book, 
    isLoading, 
    isError 
  } = useQuery<BookDetails>({
    queryKey: ['book', bookId],
    queryFn: () => getBookById(bookId)
  })

  // Update mutation
  const updateMutation = useMutation({
    mutationFn: async (values: EditBookFormValues) => {
      // Process the form data
      const payload = {
        title: values.title,
        publishDate: values.publishDate,
        basePrice: values.basePrice,
        authorIds: values.authorIds.split(',').map(id => id.trim()).filter(Boolean),
        mediums: values.mediums,
        genres: values.genres,
      }

      // Add cover image if provided
      let covers = []
      if (values.coverImage && values.coverArtistIds) {
        const imgBase64 = await fileToBase64(values.coverImage)
        covers.push({
          imgBase64,
          artistIds: values.coverArtistIds.split(',').map(id => id.trim()).filter(Boolean)
        })
      }

      return updateBook(bookId, {
        ...payload,
        covers: covers.length > 0 ? covers : undefined
      })
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['book', bookId] })
      queryClient.invalidateQueries({ queryKey: ['books'] })
      navigate({ to: '/admin/manage-books' })
    }
  })

  // Form initialization
  const form = useForm({
    defaultValues: {
      title: '',
      publishDate: '',
      basePrice: 0,
      authorIds: '',
      mediums: [] as string[],
      genres: [] as string[],
      coverImage: undefined as File | undefined,
      coverArtistIds: '',
    },
    onSubmit: async ({ value }) => {
      await updateMutation.mutateAsync(value)
    }
  })

  // Update form values when book data is loaded
  useEffect(() => {
    if (book) {
      form.reset({
        title: book.title,
        publishDate: book.publishDate.split('T')[0], // Format date for input
        basePrice: book.basePrice,
        authorIds: book.authors.map(a => a.authorPersonId).join(','),
        mediums: book.mediums,
        genres: book.genres,
        coverImage: undefined,
        coverArtistIds: '',
      })

      // Set cover preview if available
      if (book.covers && book.covers.length > 0) {
        setCoverPreview(`data:image/jpeg;base64,${book.covers[0].imgBase64}`)
      }
    }
  }, [book])

  // Loading state
  if (isLoading) {
    return (
      <div className="container mx-auto py-10 flex items-center justify-center min-h-[50vh]">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
        <span className="ml-2 text-lg">Loading book details...</span>
      </div>
    )
  }

  // Error state
  if (isError) {
    return (
      <div className="container mx-auto py-10 flex flex-col items-center justify-center min-h-[50vh] text-center">
        <AlertCircle className="h-12 w-12 text-destructive mb-4" />
        <h2 className="text-2xl font-bold mb-2">Failed to load book</h2>
        <p className="text-muted-foreground mb-4 max-w-md">
          There was an error loading the book details. The book might have been deleted or you may not have permission to view it.
        </p>
        <Button asChild>
          <Link to="/admin/manage-books">Return to Books</Link>
        </Button>
      </div>
    )
  }

  return (
    <div className="container mx-auto py-6 space-y-6">
      {/* Page header */}
      <div className="flex flex-col sm:flex-row justify-between gap-4 items-start sm:items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Edit Book</h1>
          <p className="text-muted-foreground mt-1">
            Editing "{book?.title}" <span className="text-xs bg-muted px-2 py-1 rounded-md ml-1">/{book?.slug}</span>
          </p>
        </div>
        <div className="flex gap-2">
          <Button 
            variant="outline" 
            className="shrink-0" 
            asChild
          >
            <Link to="/admin/manage-books">
              <ArrowLeft className="h-4 w-4 mr-2" />
              Cancel
            </Link>
          </Button>
          <Button 
            className="shrink-0"
            onClick={() => form.handleSubmit()}
            disabled={updateMutation.isPending}
          >
            {updateMutation.isPending ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Saving...
              </>
            ) : (
              <>
                <Save className="mr-2 h-4 w-4" />
                Save Changes
              </>
            )}
          </Button>
        </div>
      </div>

      {/* Form Tabs */}
      <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
        <TabsList className="grid grid-cols-2 w-[400px]">
          <TabsTrigger value="details">
            <BookOpen className="h-4 w-4 mr-2" />
            Book Details
          </TabsTrigger>
          <TabsTrigger value="cover">
            <ImageIcon className="h-4 w-4 mr-2" />
            Cover Image
          </TabsTrigger>
        </TabsList>

        {/* Book Details Tab */}
        <TabsContent value="details" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Basic Information</CardTitle>
              <CardDescription>
                Edit the basic details of your book.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              {/* Title Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="title-input">Title</Label>
                <form.Field
                  name="title"
                  validators={{
                    onChange: ({ value }) => !value ? 'Title is required' :
                      value.length > 100 ? 'Title must not exceed 100 characters' : undefined,
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="title-input"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Enter book title"
                        className={field.state.meta.errors?.length ? "border-destructive" : ""}
                      />
                      {field.state.meta.errors?.length ? (
                        <p className="text-sm font-medium text-destructive mt-1">
                          {field.state.meta.errors.join(', ')}
                        </p>
                      ) : null}
                    </>
                  )}
                </form.Field>
              </fieldset>

              {/* Publish Date & Price - 2 columns */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {/* Publish Date Field */}
                <fieldset disabled={updateMutation.isPending} className="space-y-2">
                  <Label htmlFor="publishDate-input">Publish Date</Label>
                  <form.Field
                    name="publishDate"
                    validators={{
                      onChange: ({ value }) => !value ? 'Publish date is required' : undefined
                    }}
                  >
                    {(field) => (
                      <>
                        <Input
                          id="publishDate-input"
                          type="date"
                          value={field.state.value}
                          onBlur={field.handleBlur}
                          onChange={(e) => field.handleChange(e.target.value)}
                          placeholder="Select publish date"
                          className={field.state.meta.errors?.length ? "border-destructive" : ""}
                        />
                        {field.state.meta.errors?.length ? (
                          <p className="text-sm font-medium text-destructive mt-1">
                            {field.state.meta.errors.join(', ')}
                          </p>
                        ) : null}
                      </>
                    )}
                  </form.Field>
                </fieldset>

                {/* Base Price Field */}
                <fieldset disabled={updateMutation.isPending} className="space-y-2">
                  <Label htmlFor="basePrice-input">Price ($)</Label>
                  <form.Field
                    name="basePrice"
                    validators={{
                      onChange: ({ value }) => value < 0 ? 'Base price must be greater than or equal to 0' : undefined
                    }}
                  >
                    {(field) => (
                      <>
                        <Input
                          id="basePrice-input"
                          type="number"
                          step="0.01"
                          min="0"
                          value={field.state.value}
                          onBlur={field.handleBlur}
                          onChange={(e) => field.handleChange(e.target.valueAsNumber || 0)}
                          placeholder="29.99"
                          className={field.state.meta.errors?.length ? "border-destructive" : ""}
                        />
                        {field.state.meta.errors?.length ? (
                          <p className="text-sm font-medium text-destructive mt-1">
                            {field.state.meta.errors.join(', ')}
                          </p>
                        ) : null}
                      </>
                    )}
                  </form.Field>
                </fieldset>
              </div>

              {/* Author IDs Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="authorIds-input">Author IDs</Label>
                <form.Field
                  name="authorIds"
                  validators={{
                    onChange: ({ value }) => !value ? 'At least one Author ID is required' : undefined
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="authorIds-input"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Enter comma-separated Author IDs"
                        className={field.state.meta.errors?.length ? "border-destructive" : ""}
                      />
                      <p className="text-[0.8rem] text-muted-foreground mt-1">
                        Enter existing Author IDs, separated by commas. (e.g., guid1, guid2)
                      </p>
                      {field.state.meta.errors?.length ? (
                        <p className="text-sm font-medium text-destructive mt-1">
                          {field.state.meta.errors.join(', ')}
                        </p>
                      ) : null}
                    </>
                  )}
                </form.Field>
              </fieldset>

              {/* Mediums Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label>Mediums</Label>
                <form.Field
                  name="mediums"
                  validators={{
                    onChange: ({ value }) => 
                      Array.isArray(value) && value.length === 0 ? 'At least one medium is required' : undefined
                  }}
                >
                  {(field) => (
                    <>
                      <ScrollArea className="h-36 border rounded-md p-2">
                        <div className="grid grid-cols-2 gap-2">
                          {MEDIUMS.map((medium) => (
                            <div key={medium.id} className="flex items-center space-x-2">
                              <Checkbox
                                id={`medium-${medium.id}`}
                                checked={field.state.value?.includes(medium.id)}
                                onCheckedChange={(checked: CheckedState) => {
                                  const currentValues = field.state.value ?? [];
                                  field.handleChange(
                                    checked === true
                                      ? [...currentValues, medium.id]
                                      : currentValues.filter((v) => v !== medium.id)
                                  );
                                }}
                              />
                              <Label htmlFor={`medium-${medium.id}`} className="font-normal">
                                {medium.label}
                              </Label>
                            </div>
                          ))}
                        </div>
                      </ScrollArea>
                      {field.state.meta.errors?.length ? (
                        <p className="text-sm font-medium text-destructive mt-1">
                          {field.state.meta.errors.join(', ')}
                        </p>
                      ) : null}
                    </>
                  )}
                </form.Field>
              </fieldset>

              {/* Genres Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label>Genres</Label>
                <form.Field
                  name="genres"
                  validators={{
                    onChange: ({ value }) => 
                      Array.isArray(value) && value.length === 0 ? 'At least one genre is required' : undefined
                  }}
                >
                  {(field) => (
                    <>
                      <ScrollArea className="h-48 border rounded-md p-2">
                        <div className="grid grid-cols-2 gap-2">
                          {GENRES.map((genre) => (
                            <div key={genre.id} className="flex items-center space-x-2">
                              <Checkbox
                                id={`genre-${genre.id}`}
                                checked={field.state.value?.includes(genre.id)}
                                onCheckedChange={(checked: CheckedState) => {
                                  const currentValues = field.state.value ?? [];
                                  field.handleChange(
                                    checked === true
                                      ? [...currentValues, genre.id]
                                      : currentValues.filter((v) => v !== genre.id)
                                  );
                                }}
                              />
                              <Label htmlFor={`genre-${genre.id}`} className="font-normal">
                                {genre.label}
                              </Label>
                            </div>
                          ))}
                        </div>
                      </ScrollArea>
                      {field.state.meta.errors?.length ? (
                        <p className="text-sm font-medium text-destructive mt-1">
                          {field.state.meta.errors.join(', ')}
                        </p>
                      ) : null}
                    </>
                  )}
                </form.Field>
              </fieldset>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Cover Image Tab */}
        <TabsContent value="cover">
          <Card>
            <CardHeader>
              <CardTitle>Book Cover</CardTitle>
              <CardDescription>
                Update the cover image for your book.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {/* Preview */}
                <div className="space-y-4">
                  <Label>Current Cover</Label>
                  <div className="relative aspect-[2/3] bg-muted rounded-md overflow-hidden">
                    {coverPreview ? (
                      <img 
                        src={coverPreview} 
                        alt="Book cover preview" 
                        className="object-cover w-full h-full" 
                      />
                    ) : (
                      <div className="flex flex-col items-center justify-center h-full text-muted-foreground">
                        <ImageIcon className="h-16 w-16 mb-2" />
                        <p>No cover image</p>
                      </div>
                    )}
                  </div>
                </div>

                {/* Upload form */}
                <div className="space-y-4">
                  <fieldset disabled={updateMutation.isPending} className="space-y-2">
                    <Label htmlFor="cover-upload-button">Upload New Cover</Label>
                    <form.Field name="coverImage">
                      {(field) => (
                        <>
                          <input
                            ref={fileInputRef}
                            id="cover-upload"
                            type="file"
                            accept="image/png, image/jpeg"
                            className="hidden"
                            onChange={(e) => {
                              const file = e.target.files?.[0];
                              field.handleChange(file ?? undefined);
                              setFileName(file?.name ?? null);
                              
                              // Create preview if a file is selected
                              if (file) {
                                const reader = new FileReader();
                                reader.onload = (e) => {
                                  setCoverPreview(e.target?.result as string);
                                };
                                reader.readAsDataURL(file);
                              }
                            }}
                          />
                          <div className="flex flex-col space-y-2">
                            <Button
                              id="cover-upload-button"
                              type="button"
                              variant="outline"
                              className="h-24 w-full border-dashed flex flex-col items-center justify-center"
                              onClick={() => fileInputRef.current?.click()}
                            >
                              <UploadIcon className="h-6 w-6 mb-2" />
                              {fileName ? `Selected: ${fileName}` : coverPreview ? "Change cover image" : "Upload Cover (PNG, JPG)"}
                            </Button>
                            <p className="text-[0.8rem] text-muted-foreground">
                              Max file size 1.5MB. Recommended dimensions: 800×1200px.
                            </p>
                          </div>
                        </>
                      )}
                    </form.Field>
                  </fieldset>

                  <fieldset disabled={updateMutation.isPending} className="space-y-2">
                    <Label htmlFor="coverArtistIds-input">Cover Artist IDs</Label>
                    <form.Field
                      name="coverArtistIds"
                      validators={{
                        onChange: ({ value }) => {
                          // Get the current cover image value from form state
                          const hasCoverImage = !!form.state.values.coverImage;
                          
                          if (hasCoverImage && !value) {
                            return 'Cover artist IDs are required when uploading a new cover';
                          }
                          return undefined;
                        }
                      }}
                    >
                      {(field) => (
                        <>
                          <Input
                            id="coverArtistIds-input"
                            value={field.state.value}
                            onBlur={field.handleBlur}
                            onChange={(e) => field.handleChange(e.target.value)}
                            placeholder="Enter comma-separated Artist IDs"
                            className={field.state.meta.errors?.length ? "border-destructive" : ""}
                          />
                          <p className="text-[0.8rem] text-muted-foreground mt-1">
                            Enter existing Artist IDs, separated by commas. Only required for new covers.
                          </p>
                          {field.state.meta.errors?.length ? (
                            <p className="text-sm font-medium text-destructive mt-1">
                              {field.state.meta.errors.join(', ')}
                            </p>
                          ) : null}
                        </>
                      )}
                    </form.Field>
                  </fieldset>
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>

      {/* Form Actions */}
      <div className="flex justify-end mt-6">
        <form.Subscribe
          selector={(state) => [state.canSubmit, state.isSubmitting, updateMutation.isPending]}
        >
          {([canSubmit, isSubmitting, isPending]) => (
            <Button 
              onClick={() => form.handleSubmit()}
              disabled={!canSubmit || isSubmitting || isPending}
              size="lg"
            >
              {isPending ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Saving...
                </>
              ) : (
                <>
                  <Save className="mr-2 h-4 w-4" />
                  Save Changes
                </>
              )}
            </Button>
          )}
        </form.Subscribe>
      </div>
    </div>
  )
} 