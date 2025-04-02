import { createFileRoute, useNavigate, redirect } from '@tanstack/react-router'
import { useForm } from '@tanstack/react-form'
import { useMutation } from '@tanstack/react-query'
import { checkUserRoleFromToken } from '@/lib/authUtils'

// UI Components
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { InfoIcon, UserIcon, LayoutDashboardIcon } from 'lucide-react'
import { createAuthor } from '../../../api/authors'

export const Route = createFileRoute('/admin/create/author')({
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
  component: CreateAuthor,
});

interface CreateAuthorFormValues {
  firstname: string;
  lastname: string;
  email: string;
  phone: string;
  royaltyrate: number;
}

function CreateAuthor() {
  const navigate = useNavigate();
  
  // TanStack Query Mutation
  const { mutate, isPending, error: mutationError } = useMutation({
    mutationFn: createAuthor,
    onSuccess: (data) => {
      console.log("Author created successfully:", data);
      alert(`Author "${data.firstName} ${data.lastName}" created successfully!`);
      navigate({ to: '/admin/manage/authors' });
    },
  });

  // TanStack Form
  const form = useForm({
    defaultValues: {
      firstname: '',
      lastname: '',
      email: '',
      phone: '',
      royaltyrate: 10, // Default royalty rate
    } as CreateAuthorFormValues,
    onSubmit: async ({ value }) => {
      console.log('Form Submitted:', value);
      mutate(value);
    },
  });

  return (
    <div className="container mx-auto p-4 md:p-8 max-w-4xl">
      <div className="flex items-center mb-6">
        <UserIcon className="h-8 w-8 mr-2 text-primary" />
        <h1 className="text-3xl font-bold">Author Creation</h1>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Left sidebar with info */}
        <Card className="md:col-span-1 h-fit hidden md:block">
          <CardHeader>
            <CardTitle className="flex items-center">
              <InfoIcon className="h-5 w-5 mr-2 text-primary" />
              Author Guide
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4 text-sm">
            <div>
              <h3 className="font-medium mb-1">Author Information</h3>
              <p className="text-muted-foreground">Fill in the basic details for the new author.</p>
            </div>
            <div>
              <h3 className="font-medium mb-1">Royalty Rate</h3>
              <p className="text-muted-foreground">Set the percentage of sales the author receives as royalties.</p>
            </div>
            <div className="pt-4 border-t">
              <Button 
                variant="outline" 
                className="w-full" 
                onClick={() => navigate({ to: '/admin/manage/authors' })}
              >
                <LayoutDashboardIcon className="mr-2 h-4 w-4" />
                Back to Authors
              </Button>
            </div>
          </CardContent>
        </Card>
        
        {/* Main form */}
        <Card className="md:col-span-2">
          <CardHeader>
            <CardTitle>Create New Author</CardTitle>
            <CardDescription>Fill in the details for the new author.</CardDescription>
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

              {/* Royalty Rate Field */}
              <fieldset disabled={isPending} className="space-y-2">
                <Label htmlFor="royaltyrate-input">Royalty Rate (%)</Label>
                <form.Field
                  name="royaltyrate"
                  validators={{
                    onChange: ({ value }) => {
                      if (value < 0) return 'Royalty rate cannot be negative';
                      if (value > 100) return 'Royalty rate cannot exceed 100%';
                      return undefined;
                    }
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="royaltyrate-input"
                        type="number"
                        step="0.1"
                        min="0"
                        max="100"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.valueAsNumber || 0)}
                        placeholder="e.g., 15"
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

              {/* Submit Button */}
              <div className="pt-4 border-t">
                {mutationError && (
                  <div className="p-3 mb-4 bg-destructive/10 border border-destructive rounded-md">
                    <p className="text-sm font-medium text-destructive">
                      Error creating author: {mutationError.message}
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
                      {isPending ? "Creating..." : "Create Author"}
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
