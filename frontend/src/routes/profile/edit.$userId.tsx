import { createFileRoute, useNavigate, redirect } from '@tanstack/react-router'
import { useForm } from '@tanstack/react-form'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import * as React from 'react'
import { useAuth } from '@/hooks/useAuth'

// UI Components
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { InfoIcon, UserIcon, LayoutDashboardIcon, Loader2, ShieldIcon } from 'lucide-react'
import { getUserProfile, updateUser } from '@/api/users'
import { UserProfile } from '@/types/user'

export const Route = createFileRoute('/profile/edit/$userId')({
  beforeLoad: async ({ params, location }) => {
    const token = localStorage.getItem('accessToken');
    if (!token) {
      throw redirect({
        to: '/auth/login',
        search: {
          redirect: location.pathname + location.search,
        },
      });
    }
    
    // Ensure users can only edit their own profile
    const payload = JSON.parse(atob(token.split('.')[1]));
    if (payload.sub !== params.userId) {
      throw redirect({
        to: '/profile',
      });
    }
  },
  component: EditProfile,
})

interface EditProfileFormValues {
  username: string;
  email: string;
  passwordHash: string;
  confirmPassword: string;
}

function EditProfile() {
  const { userId } = Route.useParams();
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();
  const queryClient = useQueryClient();
  
  // Fetch user profile data
  const { data: profile, isLoading, isError, error } = useQuery<UserProfile>({
    queryKey: ['profile', userId],
    queryFn: () => getUserProfile(userId),
    enabled: !!userId && isAuthenticated,
  });
  
  // TanStack Form
  const form = useForm({
    defaultValues: React.useMemo(() => {
      if (!profile) return {} as EditProfileFormValues;
      
      return {
        username: profile.username || '',
        email: profile.email || '',
        passwordHash: '',
        confirmPassword: '',
      };
    }, [profile]),
    onSubmit: async ({ value }) => {
      // Only include fields that have values
      const updateData: { 
        username?: string; 
        email?: string; 
        passwordHash?: string;
      } = {};
      
      if (value.username) updateData.username = value.username;
      if (value.email) updateData.email = value.email;
      if (value.passwordHash) updateData.passwordHash = value.passwordHash;
      
      updateMutation.mutate({
        userId,
        userData: updateData
      });
    },
  });

  // Update form values when profile data is loaded
  React.useEffect(() => {
    if (profile) {
      form.reset({
        username: profile.username || '',
        email: profile.email || '',
        passwordHash: '',
        confirmPassword: '',
      });
    }
  }, [profile, form]);
  
  // TanStack Query Mutation
  const updateMutation = useMutation({
    mutationFn: ({ userId, userData }: { 
      userId: string; 
      userData: { 
        username?: string; 
        email?: string; 
        passwordHash?: string;
      }
    }) => updateUser(userId, userData),
    onSuccess: (data) => {
      console.log("Profile updated successfully:", data);
      // Invalidate profile query cache to force a refetch when returning to profile page
      queryClient.invalidateQueries({ queryKey: ['profile', userId] });
      navigate({ to: '/profile' });
    },
    onError: (error) => {
      // Just log the error - we'll handle the display in the UI
      console.error("Error updating profile:", error);
    }
  });

  if (isLoading || !profile) {
    return (
      <div className="flex justify-center items-center h-[calc(100vh-6rem)]">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
        <span className="ml-2">Loading profile...</span>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="container mx-auto p-4 md:p-8 text-center">
        <h1 className="text-3xl font-bold text-destructive mb-4">Error</h1>
        <p className="mb-4">Failed to load profile: {error instanceof Error ? error.message : 'Unknown error'}</p>
        <Button onClick={() => navigate({ to: '/profile' })}>
          Return to Profile
        </Button>
      </div>
    );
  }

  return (
    <div className="container mx-auto p-4 md:p-8 max-w-4xl">
      <div className="flex items-center mb-6">
        <UserIcon className="h-8 w-8 mr-2 text-primary" />
        <h1 className="text-3xl font-bold">Edit Profile</h1>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Left sidebar with info */}
        <Card className="md:col-span-1 h-fit hidden md:block">
          <CardHeader>
            <CardTitle className="flex items-center">
              <InfoIcon className="h-5 w-5 mr-2 text-primary" />
              Profile Guide
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4 text-sm">
            <div>
              <h3 className="font-medium mb-1">Profile Information</h3>
              <p className="text-muted-foreground">Update your profile details as needed.</p>
            </div>
            <div>
              <h3 className="font-medium mb-1">Password</h3>
              <p className="text-muted-foreground">Leave password fields empty if you don't want to change it.</p>
            </div>
            <div className="flex items-center mt-4 p-3 bg-primary/10 rounded border border-primary/20">
              <ShieldIcon className="h-5 w-5 mr-2 text-primary" />
              <p className="text-sm">Your role is <span className="font-medium">{profile.role}</span></p>
            </div>
            <div className="pt-4 border-t">
              <Button 
                variant="outline" 
                className="w-full" 
                onClick={() => navigate({ to: '/profile' })}
              >
                <LayoutDashboardIcon className="mr-2 h-4 w-4" />
                Back to Profile
              </Button>
            </div>
          </CardContent>
        </Card>
        
        {/* Main form */}
        <Card className="md:col-span-2">
          <CardHeader>
            <CardTitle>Edit Profile</CardTitle>
            <CardDescription>Update your personal information</CardDescription>
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
              {/* Username Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="username-input">Username</Label>
                <form.Field
                  name="username"
                  validators={{
                    onChange: ({ value }) => {
                      if (value && value.length > 50) return 'Username must be less than 50 characters';
                      return undefined;
                    },
                  }}
                >
                  {(field) => {
                    // Check if there's a duplicate username error
                    const isDuplicateError = updateMutation.error instanceof Error && 
                      (updateMutation.error.message.includes('duplicate key') || 
                       updateMutation.error.message.toLowerCase().includes('username is already taken'));
                    
                    return (
                      <>
                        <Input
                          id="username-input"
                          value={field.state.value}
                          onBlur={field.handleBlur}
                          onChange={(e) => field.handleChange(e.target.value)}
                          placeholder="Enter username"
                          className={field.state.meta.errors?.length || isDuplicateError ? "border-destructive" : ""}
                        />
                        {field.state.meta.errors?.length ? (
                          <p className="text-sm font-medium text-destructive mt-1">
                            {field.state.meta.errors.join(', ')}
                          </p>
                        ) : isDuplicateError ? (
                          <p className="text-sm font-medium text-destructive mt-1">
                            This username is already taken
                          </p>
                        ) : null}
                      </>
                    );
                  }}
                </form.Field>
              </fieldset>

              {/* Email Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="email-input">Email</Label>
                <form.Field
                  name="email"
                  validators={{
                    onChange: ({ value }) => {
                      if (value) {
                        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                        if (!emailRegex.test(value)) return 'Please enter a valid email address';
                        if (value.length > 100) return 'Email must be less than 100 characters';
                      }
                      return undefined;
                    }
                  }}
                >
                  {(field) => {
                    // Check if there's a duplicate email error
                    const isDuplicateError = updateMutation.error instanceof Error && 
                      (updateMutation.error.message.includes('duplicate key') || 
                       updateMutation.error.message.toLowerCase().includes('email is already taken'));
                    
                    return (
                      <>
                        <Input
                          id="email-input"
                          type="email"
                          value={field.state.value}
                          onBlur={field.handleBlur}
                          onChange={(e) => field.handleChange(e.target.value)}
                          placeholder="Enter email address"
                          className={field.state.meta.errors?.length || isDuplicateError ? "border-destructive" : ""}
                        />
                        {field.state.meta.errors?.length ? (
                          <p className="text-sm font-medium text-destructive mt-1">
                            {field.state.meta.errors.join(', ')}
                          </p>
                        ) : isDuplicateError ? (
                          <p className="text-sm font-medium text-destructive mt-1">
                            This email is already taken
                          </p>
                        ) : null}
                      </>
                    );
                  }}
                </form.Field>
              </fieldset>

              {/* Password Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="password-input">New Password</Label>
                <form.Field
                  name="passwordHash"
                  validators={{
                    onChange: ({ value }) => {
                      if (value) {
                        if (value.length < 8) return 'Password must be at least 8 characters';
                        if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/.test(value)) {
                          return 'Password must contain at least one uppercase letter, one lowercase letter, and one number';
                        }
                      }
                      return undefined;
                    }
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="password-input"
                        type="password"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Leave blank to keep current password"
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

              {/* Confirm Password Field */}
              <fieldset disabled={updateMutation.isPending} className="space-y-2">
                <Label htmlFor="confirm-password-input">Confirm New Password</Label>
                <form.Field
                  name="confirmPassword"
                  validators={{
                    onChange: ({ value }) => {
                      const passwordValue = form.getFieldValue('passwordHash');
                      if (passwordValue && value !== passwordValue) {
                        return 'Passwords do not match';
                      }
                      return undefined;
                    }
                  }}
                >
                  {(field) => (
                    <>
                      <Input
                        id="confirm-password-input"
                        type="password"
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        onChange={(e) => field.handleChange(e.target.value)}
                        placeholder="Confirm new password"
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
                {updateMutation.error && (
                  <div className="p-3 mb-4 bg-destructive/10 border border-destructive rounded-md">
                    <p className="text-sm font-medium text-destructive">
                      {updateMutation.error instanceof Error && 
                       (updateMutation.error.message.toLowerCase().includes('username is already taken') || 
                        updateMutation.error.message.includes('duplicate key') && updateMutation.error.message.includes('Username'))
                        ? 'Username is already taken. Please choose a different username.'
                        : updateMutation.error instanceof Error && 
                          (updateMutation.error.message.toLowerCase().includes('email is already taken') || 
                           updateMutation.error.message.includes('duplicate key') && updateMutation.error.message.includes('Email'))
                          ? 'Email address is already taken. Please use a different email.'
                          : `Error updating profile: ${updateMutation.error instanceof Error ? updateMutation.error.message : 'Unknown error'}`}
                    </p>
                  </div>
                )}
                
                <div className="flex space-x-3">
                  <Button 
                    variant="outline" 
                    onClick={() => navigate({ to: '/profile' })}
                    className="flex-1"
                  >
                    Cancel
                  </Button>
                  
                  <form.Subscribe
                    selector={(state) => [state.canSubmit, state.isSubmitting]}
                  >
                    {([canSubmit, isSubmitting]) => (
                      <Button 
                        type="submit" 
                        disabled={!canSubmit || isSubmitting || updateMutation.isPending} 
                        className="flex-1"
                      >
                        {updateMutation.isPending ? "Updating..." : "Update Profile"}
                      </Button>
                    )}
                  </form.Subscribe>
                </div>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
