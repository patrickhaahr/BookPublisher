import { createFileRoute, redirect, Link, useNavigate } from '@tanstack/react-router'
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
  UploadIcon,
  CalendarIcon,
  InfoIcon
} from 'lucide-react'
import { type CheckedState } from "@radix-ui/react-checkbox"
import { ScrollArea } from '../../components/ui/scroll-area'
import { GENRES, MEDIUMS } from '../../constants/bookOptions'
import { Calendar } from '../../components/ui/calendar'
import { format } from 'date-fns'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "../../components/ui/popover"
import { Alert, AlertDescription, AlertTitle } from "../../components/ui/alert"
import { cn } from "../../lib/utils"
import { checkUserRoleFromToken } from '@/lib/authUtils'

export const Route = createFileRoute('/admin/edit-book/$bookId')({
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
  component: EditBookPage,
})

function EditBookPage() {
  const { bookId } = Route.useParams()
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [coverPreview, setCoverPreview] = useState<string | null>(null)
  const [activeTab, setActiveTab] = useState('details')
  const [fileName, setFileName] = useState<string | null>(null)
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(undefined)
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
      // Create an empty payload that will only include properties that are defined
      const payload: Record<string, any> = {}
      
      // Only add non-null properties to the payload
      if (values.title !== undefined && values.title !== null) {
        payload.title = values.title
      }
      
      if (values.publishDate !== undefined && values.publishDate !== null) {
        payload.publishDate = values.publishDate
      }
      
      if (values.basePrice !== undefined && values.basePrice !== null) {
        payload.basePrice = values.basePrice
      }
      
      if (values.authorIds !== undefined && values.authorIds !== null) {
        payload.authorIds = values.authorIds.split(',').map(id => id.trim()).filter(Boolean)
      }
      
      if (values.mediums !== undefined && values.mediums !== null) {
        payload.mediums = values.mediums
      }
      
      if (values.genres !== undefined && values.genres !== null) {
        payload.genres = values.genres
      }

      // Add cover image if provided
      if (values.coverImage && values.coverArtistIds) {
        const imgBase64 = await fileToBase64(values.coverImage)
        payload.covers = [{
          imgBase64,
          artistIds: values.coverArtistIds.split(',').map(id => id.trim()).filter(Boolean)
        }]
      }

      return updateBook(bookId, payload)
    },
    onSuccess: () => {
      // Ensure all relevant queries are invalidated with proper configuration
      queryClient.invalidateQueries({ queryKey: ['book', bookId], refetchType: 'all' })
      queryClient.invalidateQueries({ queryKey: ['books'], refetchType: 'all' })
      navigate({ to: '/admin/manage-books' })
    }
  })

  // Form initialization
  const form = useForm({
    defaultValues: {
      title: null as string | null,
      publishDate: null as string | null,
      basePrice: null as number | null,
      authorIds: null as string | null,
      mediums: null as string[] | null,
      genres: null as string[] | null,
      coverImage: undefined as File | undefined,
      coverArtistIds: null as string | null,
    },
    onSubmit: async ({ value }) => {
      await updateMutation.mutateAsync(value)
    }
  })

  // Update form values when book data is loaded
  useEffect(() => {
    if (book) {
      form.reset({
        title: null,
        publishDate: null,
        basePrice: null,
        authorIds: null,
        mediums: null,
        genres: null,
        coverImage: undefined,
        coverArtistIds: null,
      })

      // Set cover preview if available
      if (book.covers && book.covers.length > 0) {
        setCoverPreview(`data:image/jpeg;base64,${book.covers[0].imgBase64}`)
      }
      
      // Set the date for the calendar
      if (book.publishDate) {
        setSelectedDate(new Date(book.publishDate))
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

      {/* Partial update notice */}
      <Alert className="bg-blue-50 dark:bg-blue-950 mb-6">
        <InfoIcon className="h-4 w-4" />
        <AlertTitle>Partial Updates Supported</AlertTitle>
        <AlertDescription>
          You only need to fill the fields you want to update. Empty fields will keep their existing values.
        </AlertDescription>
      </Alert>

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
                Edit the basic details of your book. Leave fields empty to keep their existing values.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              {/* Title Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="title-input">Title</Label>
                <form.Field
                  name="title"
                  validators={{
                    onChange: ({ value }) => 
                      value && value.length > 100 ? 'Title must not exceed 100 characters' : undefined,
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="title-input"
                        value={field.state.value ?? ''}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value || null)}
                        placeholder={book?.title ?? "Enter book title"}
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
                  >
                    {(field) => (
                      <>
                        <Popover>
                          <PopoverTrigger asChild>
                            <Button
                              id="publishDate-input"
                              variant={"outline"}
                              className={cn(
                                "w-full justify-start text-left font-normal",
                                !selectedDate && "text-muted-foreground",
                                field.state.meta.errors?.length && "border-destructive"
                              )}
                            >
                              <CalendarIcon className="mr-2 h-4 w-4" />
                              {selectedDate ? (
                                format(selectedDate, "PPP")
                              ) : (
                                <span>{book?.publishDate ? format(new Date(book.publishDate), "PPP") : "Select date"}</span>
                              )}
                            </Button>
                          </PopoverTrigger>
                          <PopoverContent className="w-auto p-0" align="start">
                            <Calendar
                              mode="single"
                              selected={selectedDate}
                              onSelect={(date) => {
                                setSelectedDate(date);
                                field.handleChange(date ? format(date, "yyyy-MM-dd") : null);
                              }}
                              initialFocus
                            />
                          </PopoverContent>
                        </Popover>
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
                      onChange: ({ value }) => value !== null && value < 0 ? 'Base price must be greater than or equal to 0' : undefined
                    }}
                  >
                    {(field) => (
                      <>
                        <Input
                          id="basePrice-input"
                          type="number"
                          step="0.01"
                          min="0"
                          value={field.state.value ?? ''}
                          onBlur={field.handleBlur}
                          onChange={(e) => {
                            const value = e.target.value === '' ? null : e.target.valueAsNumber
                            field.handleChange(value)
                          }}
                          placeholder={book?.basePrice?.toString() ?? "29.99"}
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
                >
                  {(field) => (
                    <>
                      <Input
                        id="authorIds-input"
                        value={field.state.value ?? ''}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value || null)}
                        placeholder={book?.authors?.map(a => a.authorPersonId).join(', ') ?? "Enter comma-separated Author IDs"}
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
                >
                  {(field) => (
                    <>
                      <ScrollArea className="h-36 border rounded-md p-2">
                        <div className="grid grid-cols-2 gap-2">
                          {MEDIUMS.map((medium) => (
                            <div key={medium.id} className="flex items-center space-x-2">
                              <Checkbox
                                id={`medium-${medium.id}`}
                                checked={field.state.value?.includes(medium.id) ?? book?.mediums?.includes(medium.id) ?? false}
                                onCheckedChange={(checked: CheckedState) => {
                                  // Initialize with current field value or book value or empty array
                                  const currentValues = field.state.value ?? [...(book?.mediums ?? [])];
                                  
                                  const newValues = checked === true
                                    ? [...currentValues, medium.id]
                                    : currentValues.filter((v) => v !== medium.id);
                                  
                                  // Only set the field value if it's different from book value
                                  field.handleChange(newValues.length > 0 ? newValues : null);
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
                >
                  {(field) => (
                    <>
                      <ScrollArea className="h-48 border rounded-md p-2">
                        <div className="grid grid-cols-2 gap-2">
                          {GENRES.map((genre) => (
                            <div key={genre.id} className="flex items-center space-x-2">
                              <Checkbox
                                id={`genre-${genre.id}`}
                                checked={field.state.value?.includes(genre.id) ?? book?.genres?.includes(genre.id) ?? false}
                                onCheckedChange={(checked: CheckedState) => {
                                  // Initialize with current field value or book value or empty array
                                  const currentValues = field.state.value ?? [...(book?.genres ?? [])];
                                  
                                  const newValues = checked === true
                                    ? [...currentValues, genre.id]
                                    : currentValues.filter((v) => v !== genre.id);
                                  
                                  // Only set the field value if it's different from book value
                                  field.handleChange(newValues.length > 0 ? newValues : null);
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
                            value={field.state.value ?? ''}
                            onBlur={field.handleBlur}
                            onChange={(e) => field.handleChange(e.target.value || null)}
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
          selector={(state) => [state.isSubmitting, updateMutation.isPending]}
        >
          {([isSubmitting, isPending]) => (
            <Button 
              onClick={() => form.handleSubmit()}
              disabled={isSubmitting || isPending}
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