import { createFileRoute, useNavigate, redirect } from '@tanstack/react-router'
import { useForm } from '@tanstack/react-form'
import { useMutation } from '@tanstack/react-query'
import { checkUserRoleFromToken } from '@/lib/authUtils'

// UI Components
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { InfoIcon, PaintbrushIcon, LayoutDashboardIcon } from 'lucide-react'
import { createArtist } from '../../../api/artists'

export const Route = createFileRoute('/admin/create/artist')({
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
  component: CreateArtist,
});

interface CreateArtistFormValues {
  firstname: string;
  lastname: string;
  email: string;
  phone: string;
  portfoliourl: string;
}

function CreateArtist() {
  const navigate = useNavigate();
  
  // TanStack Query Mutation
  const { mutate, isPending, error: mutationError } = useMutation({
    mutationFn: createArtist,
    onSuccess: (data) => {
      console.log("Artist created successfully:", data);
      alert(`Artist "${data.firstName} ${data.lastName}" created successfully!`);
      navigate({ to: '/admin/manage/artists' });
    },
  });

  // TanStack Form
  const form = useForm({
    defaultValues: {
      firstname: '',
      lastname: '',
      email: '',
      phone: '',
      portfoliourl: 'https://',
    } as CreateArtistFormValues,
    onSubmit: async ({ value }) => {
      console.log('Form Submitted:', value);
      mutate(value);
    },
  });

  return (
    <div className="container mx-auto p-4 md:p-8 max-w-4xl">
      <div className="flex items-center mb-6">
        <PaintbrushIcon className="h-8 w-8 mr-2 text-primary" />
        <h1 className="text-3xl font-bold">Artist Creation</h1>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Left sidebar with info */}
        <Card className="md:col-span-1 h-fit hidden md:block">
          <CardHeader>
            <CardTitle className="flex items-center">
              <InfoIcon className="h-5 w-5 mr-2 text-primary" />
              Artist Guide
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4 text-sm">
            <div>
              <h3 className="font-medium mb-1">Artist Information</h3>
              <p className="text-muted-foreground">Fill in the basic details for the new cover artist.</p>
            </div>
            <div>
              <h3 className="font-medium mb-1">Portfolio URL</h3>
              <p className="text-muted-foreground">Include a link to the artist's portfolio website. Must start with "https://".</p>
            </div>
            <div className="pt-4 border-t">
              <Button 
                variant="outline" 
                className="w-full" 
                onClick={() => navigate({ to: '/admin/manage/artists' })}
              >
                <LayoutDashboardIcon className="mr-2 h-4 w-4" />
                Back to Artists
              </Button>
            </div>
          </CardContent>
        </Card>
        
        {/* Main form */}
        <Card className="md:col-span-2">
          <CardHeader>
            <CardTitle>Create New Artist</CardTitle>
            <CardDescription>Fill in the details for the new cover artist.</CardDescription>
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
              {/* First Name Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="firstname-input">First Name</Label>
                <form.Field
                  name="firstname"
                  validators={{
                    onChange: ({ value }) => !value ? 'First name is required' : undefined,
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="firstname-input"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Enter first name"
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

              {/* Last Name Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="lastname-input">Last Name</Label>
                <form.Field
                  name="lastname"
                  validators={{
                    onChange: ({ value }) => !value ? 'Last name is required' : undefined,
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="lastname-input"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Enter last name"
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

              {/* Email Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="email-input">Email</Label>
                <form.Field
                  name="email"
                  validators={{
                    onChange: ({ value }) => {
                      if (!value) return 'Email is required';
                      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                      if (!emailRegex.test(value)) return 'Please enter a valid email address';
                      return undefined;
                    }
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="email-input"
                        type="email"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Enter email address"
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

              {/* Phone Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="phone-input">Phone</Label>
                <form.Field
                  name="phone"
                >
                  {(field) => (
                    <>
                      <Input
                        id="phone-input"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Enter phone number (optional)"
                      />
                    </>
                  )}
                </form.Field>
              </fieldset>

              {/* Portfolio URL Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="portfoliourl-input">Portfolio URL</Label>
                <form.Field
                  name="portfoliourl"
                  validators={{
                    onChange: ({ value }) => {
                      if (!value) return 'Portfolio URL is required';
                      if (!value.startsWith('https://')) return 'URL must start with "https://"';
                      return undefined;
                    }
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="portfoliourl-input"
                        type="url"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="e.g., https://www.portfolio-example.com"
                        className={field.state.meta.errors?.length ? "border-destructive" : ""}
                      />
                      <p className="text-[0.8rem] text-muted-foreground mt-1">
                        Must include the full URL with "https://" prefix
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
                      Error creating artist: {mutationError.message}
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
                      {isPending ? "Creating..." : "Create Artist"}
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
