import { createFileRoute, useNavigate, redirect } from '@tanstack/react-router'
import { useForm } from '@tanstack/react-form'
import { useMutation } from '@tanstack/react-query'
import * as React from 'react'
import { type CheckedState } from "@radix-ui/react-checkbox"
import { checkUserRoleFromToken } from '@/lib/authUtils' // Import the helper

// UI Components
import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Checkbox } from "@/components/ui/checkbox"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"
import { ScrollArea } from "@/components/ui/scroll-area"
import { cn } from "@/lib/utils"
import { format } from "date-fns"
import { CalendarIcon, UploadIcon, InfoIcon, BookOpenIcon, LayoutDashboardIcon } from 'lucide-react'

// Available options based on backend enums
const ALL_MEDIUMS = [
  { id: 'Print', label: 'Print' },
  { id: 'Magazine', label: 'Magazine' },
  { id: 'EBook', label: 'E-Book' },
  { id: 'AudioBook', label: 'Audio Book' },
  { id: 'Novel', label: 'Novel' },
  { id: 'LightNovel', label: 'Light Novel' },
  { id: 'WebNovel', label: 'Web Novel' },
  { id: 'GraphicNovel', label: 'Graphic Novel' },
  { id: 'Comic', label: 'Comic' },
  { id: 'Manga', label: 'Manga' },
  { id: 'Manhwa', label: 'Manhwa' },
  { id: 'Manhua', label: 'Manhua' },
] as const

const ALL_GENRES = [
  { id: 'Fiction', label: 'Fiction' },
  { id: 'NonFiction', label: 'Non-Fiction' },
  { id: 'Fantasy', label: 'Fantasy' },
  { id: 'ScienceFiction', label: 'Science Fiction' },
  { id: 'Mystery', label: 'Mystery' },
  { id: 'Romance', label: 'Romance' },
  { id: 'Thriller', label: 'Thriller' },
  { id: 'Horror', label: 'Horror' },
  { id: 'HistoricalFiction', label: 'Historical Fiction' },
  { id: 'Adventure', label: 'Adventure' },
  { id: 'YoungAdult', label: 'Young Adult' },
  { id: 'Childrens', label: 'Children\'s' },
  { id: 'Biography', label: 'Biography' },
  { id: 'SelfHelp', label: 'Self Help' },
  { id: 'Poetry', label: 'Poetry' },
  { id: 'Science', label: 'Science' },
  { id: 'Travel', label: 'Travel' },
  { id: 'Humor', label: 'Humor' },
  { id: 'Programming', label: 'Programming' },
  { id: 'Finance', label: 'Finance' },
  { id: 'Epic', label: 'Epic' },
  { id: 'DarkFantasy', label: 'Dark Fantasy' },
] as const

// Form values interface
interface CreateBookFormValues {
  title: string;
  publishDate?: Date;
  basePrice: number;
  authorIds: string; 
  mediums: string[];
  genres: string[];
  coverImage?: File;
  coverArtistIds: string;
}

// API types
interface CreateBookApiResponse {
  bookId: string;
  title: string;
  slug: string;
}

interface CreateBookApiPayload {
  title: string;
  publishDate?: string; 
  basePrice: number;
  authorIds: string[];
  mediums: string[];
  genres: string[];
  covers: { imgBase64: string; artistIds: string[] }[];
}

// API Function
const createBookApi = async (payload: CreateBookApiPayload): Promise<CreateBookApiResponse> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch('http://localhost:5094/api/v1/books', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    let errorData;
    try {
      errorData = await response.json();
    } catch (e) {
      throw new Error(`HTTP error ${response.status}: ${response.statusText}`);
    }
    throw new Error(errorData?.title || errorData?.message || `HTTP error ${response.status}`);
  }

  return await response.json();
};

// Helper function to convert File to base64
const fileToBase64 = (file: File): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      if (typeof reader.result === 'string') {
        // Remove the "data:*/*;base64," prefix
        resolve(reader.result.split(',')[1]);
      } else {
        reject(new Error('Failed to read file as base64 string'));
      }
    };
    reader.onerror = (error) => reject(error);
  });
};

export const Route = createFileRoute('/admin/create-book')({
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
  component: CreateBook,
});

function CreateBook() {
  const navigate = useNavigate();
  const [fileName, setFileName] = React.useState<string | null>(null);
  const [formErrors, setFormErrors] = React.useState<Record<string, string>>({});
  const fileInputRef = React.useRef<HTMLInputElement>(null);

  // TanStack Query Mutation
  const { mutate, isPending, error: mutationError } = useMutation({
    mutationFn: createBookApi,
    onSuccess: (data) => {
      console.log("Book created successfully:", data);
      alert(`Book "${data.title}" created successfully!`);
      navigate({ to: '/admin' });
    },
  });

  // TanStack Form
  const form = useForm({
    defaultValues: {
      title: '',
      publishDate: undefined,
      basePrice: 0,
      authorIds: '',
      mediums: [] as string[],
      genres: [] as string[],
      coverImage: undefined,
      coverArtistIds: '',
    } as CreateBookFormValues,
    onSubmit: async ({ value }) => {
      console.log('Form Submitted (Raw):', value);
      
      // Clear previous errors
      setFormErrors({});
      
      // Validate cover image if provided
      let imgBase64: string | undefined = undefined;
      if (value.coverImage) {
        if (!value.coverImage.type.startsWith('image/')) {
          setFormErrors(prev => ({ ...prev, coverImage: 'File must be an image (PNG, JPG).' }));
          return;
        }
        
        if (value.coverImage.size > 1500000) {
          setFormErrors(prev => ({ ...prev, coverImage: 'Max file size is 1.5MB.' }));
          return;
        }

        try {
          imgBase64 = await fileToBase64(value.coverImage);
        } catch (error) {
          console.error("Error converting image to base64:", error);
          setFormErrors(prev => ({ ...prev, coverImage: 'Failed to process image file.' }));
          return;
        }
      }

      // Prepare API payload
      const requestData: CreateBookApiPayload = {
        title: value.title,
        publishDate: value.publishDate?.toISOString(),
        basePrice: value.basePrice,
        authorIds: value.authorIds.split(',').map(id => id.trim()).filter(Boolean),
        mediums: value.mediums,
        genres: value.genres,
        covers: imgBase64 && value.coverArtistIds ? [{
          imgBase64,
          artistIds: value.coverArtistIds.split(',').map(id => id.trim()).filter(Boolean),
        }] : [],
      };

      console.log('API Request:', JSON.stringify(requestData));

      // Submit to API
      mutate(requestData);
    },
  });

  return (
    <div className="container mx-auto p-4 md:p-8 max-w-4xl">
      <div className="flex items-center mb-6">
        <BookOpenIcon className="h-8 w-8 mr-2 text-primary" />
        <h1 className="text-3xl font-bold">Book Creation</h1>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Left sidebar with info */}
        <Card className="md:col-span-1 h-fit hidden md:block">
          <CardHeader>
            <CardTitle className="flex items-center">
              <InfoIcon className="h-5 w-5 mr-2 text-primary" />
              Publishing Guide
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4 text-sm">
            <div>
              <h3 className="font-medium mb-1">Book Title</h3>
              <p className="text-muted-foreground">Keep titles concise but descriptive, maximum 100 characters.</p>
            </div>
            <div>
              <h3 className="font-medium mb-1">Publish Date</h3>
              <p className="text-muted-foreground">Historical dates only, future publishing not supported.</p>
            </div>
            <div>
              <h3 className="font-medium mb-1">Price</h3>
              <p className="text-muted-foreground">Set a competitive base price, discounts can be applied later.</p>
            </div>
            <div>
              <h3 className="font-medium mb-1">Cover Image</h3>
              <p className="text-muted-foreground">Upload high-quality JPG or PNG images under 1.5MB.</p>
            </div>
            <div className="pt-4 border-t">
              <Button 
                variant="outline" 
                className="w-full" 
                onClick={() => navigate({ to: '/admin' })}
              >
                <LayoutDashboardIcon className="mr-2 h-4 w-4" />
                Back to Dashboard
              </Button>
            </div>
          </CardContent>
        </Card>
        
        {/* Main form */}
        <Card className="md:col-span-2">
          <CardHeader>
            <CardTitle>Create New Book</CardTitle>
            <CardDescription>Fill in the details for the new book entry.</CardDescription>
          </CardHeader>
          <CardContent>
            <form
              onSubmit={(e) => {
                e.preventDefault();
                e.stopPropagation();
                form.handleSubmit();
              }}
              className="space-y-6"
            >
              {/* Title Field */}
              <fieldset disabled={isPending} className="space-y-2">
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

              {/* Publish Date Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label>Publish Date</Label>
                <form.Field
                  name="publishDate"
                  validators={{
                    onChange: ({ value }) => !value ? 'Publish date is required' :
                      value > new Date() ? 'Publish date cannot be in the future' : undefined
                  }}
                >
                  {(field) => (
                    <>
                      <Popover>
                        <PopoverTrigger asChild>
                          <Button
                            variant={"outline"}
                            className={cn(
                              "w-full justify-start text-left font-normal",
                              !field.state.value && "text-muted-foreground",
                              field.state.meta.errors?.length ? "border-destructive" : ""
                            )}
                          >
                            <CalendarIcon className="mr-2 h-4 w-4" />
                            {field.state.value ? format(field.state.value, "PPP") : <span>Pick a date</span>}
                          </Button>
                        </PopoverTrigger>
                        <PopoverContent className="w-auto p-0" align="start">
                          <Calendar
                            mode="single"
                            selected={field.state.value}
                            onSelect={(date) => field.handleChange(date)}
                            disabled={(date) =>
                              date > new Date() || date < new Date("1900-01-01")
                            }
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
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="basePrice-input">Base Price ($)</Label>
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
                        placeholder="e.g., 29.99"
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

              {/* Author IDs Field */}
              <fieldset disabled={isPending} className="space-y-2">
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

              {/* Two-column layout for Mediums and Genres */}
              <div className="grid grid-cols-1 gap-4">
                {/* Mediums Field */}
                <fieldset disabled={isPending} className="space-y-2">
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
                            {ALL_MEDIUMS.map((medium) => (
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
                <fieldset disabled={isPending} className="space-y-2">
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
                            {ALL_GENRES.map((genre) => (
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
              </div>

              {/* Cover Image Upload */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="cover-upload-button">Cover Image</Label>
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
                          setFormErrors({});
                        }}
                      />
                      <div className="flex flex-col space-y-2">
                        <Button
                          id="cover-upload-button"
                          type="button"
                          variant="outline"
                          className={cn(
                            "h-24 w-full border-dashed flex flex-col items-center justify-center",
                            formErrors.coverImage ? "border-destructive" : ""
                          )}
                          onClick={() => fileInputRef.current?.click()}
                        >
                          <UploadIcon className="h-6 w-6 mb-2" />
                          {fileName ? `Selected: ${fileName}` : "Upload Cover (PNG, JPG)"}
                        </Button>
                        <p className="text-[0.8rem] text-muted-foreground">
                          Max file size 1.5MB. Recommended dimensions: 800×1200px.
                        </p>
                        {formErrors.coverImage && (
                          <p className="text-sm font-medium text-destructive">
                            {formErrors.coverImage}
                          </p>
                        )}
                      </div>
                    </>
                  )}
                </form.Field>
              </fieldset>

              {/* Cover Artist IDs */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="coverArtistIds-input">Cover Artist IDs</Label>
                <form.Field
                  name="coverArtistIds"
                  validators={{
                    onChange: ({ value }) => !value ? 'At least one Cover Artist ID is required' : undefined
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
                        Enter existing Artist IDs, separated by commas. (e.g., guidA, guidB)
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

              {/* Submit Button */}
              <div className="pt-4 border-t">
                {mutationError && (
                  <div className="p-3 mb-4 bg-destructive/10 border border-destructive rounded-md">
                    <p className="text-sm font-medium text-destructive">
                      Error creating book: {mutationError.message}
                    </p>
                  </div>
                )}
                
                <form.Subscribe
                  selector={(state) => [state.canSubmit, state.isSubmitting]}
                >
                  {([canSubmit, isSubmitting]) => (
                    <Button 
                      type="submit" 
                      disabled={!canSubmit || isSubmitting || isPending} 
                      className="w-full"
                    >
                      {isPending ? "Creating..." : "Create Book"}
                    </Button>
                  )}
                </form.Subscribe>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
