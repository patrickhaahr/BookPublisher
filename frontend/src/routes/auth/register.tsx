import { useForm } from '@tanstack/react-form';
import { useMutation } from '@tanstack/react-query';
import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useAuth } from '@/hooks/useAuth';

// Import API function
import { registerUser } from '@/api';

export const Route = createFileRoute('/auth/register')({
  component: Register,
});

function Register() {
  const navigate = useNavigate();
  const { login } = useAuth();
  
  // TanStack Form setup
  const form = useForm({
    defaultValues: {
      username: '',
      email: '',
      password: '',
      confirmPassword: '',
    },
    onSubmit: async ({ value }) => {  
      // Remove confirmPassword before sending to API
      const { confirmPassword, ...registrationData } = value;
      mutate(registrationData);
    },
  });

  // TanStack Query mutation for registration
  const { mutate, isPending, error, isSuccess } = useMutation({
    mutationFn: registerUser,
    onSuccess: (data) => {
      // Handle successful registration
      console.log('Registration successful:', data);
      // Log user in automatically after registration
      login(data.accessToken, data.refreshToken, data.userId);
      // Redirect to home page
      navigate({ to: '/' });
    },
    onError: (err) => {
      console.error('Registration error:', err.message);
    },
  });

  return (
    <div className="min-h-auto flex items-center justify-center py-12">
      <Card className="w-full max-w-md mx-auto">
        <CardHeader>
          <CardTitle className="text-2xl font-bold text-center">Register</CardTitle>
        </CardHeader>
        <CardContent>
          <form
            onSubmit={(e) => {
              e.preventDefault();
              e.stopPropagation();
              form.handleSubmit();
            }}
            className="space-y-4"
          >
            {/* Username Field */}
            <div className="space-y-2">
              <Label htmlFor="username">Username</Label>
              <form.Field
                name="username"
                validators={{
                  onChange: ({ value }) =>
                    !value
                      ? 'Username is required'
                      : value.length < 3
                        ? 'Username must be at least 3 characters'
                        : undefined,
                }}
              >
                {(field) => (
                  <div>
                    <Input
                      id="username"
                      type="text"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      placeholder="Enter your username"
                      disabled={isPending}
                      className={field.state.meta.errors.length > 0 ? 'border-red-500' : ''}
                    />
                    {field.state.meta.errors.map((error) => (
                      <p key={error as string} className="text-red-500 text-sm">{error}</p>
                    ))}
                  </div>
                )}
              </form.Field>
            </div>

            {/* Email Field */}
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <form.Field
                name="email"
                validators={{
                  onChange: ({ value }) =>
                    !value
                      ? 'Email is required'
                      : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)
                        ? 'Invalid email format'
                        : undefined,
                }}
              >
                {(field) => (
                  <div>
                    <Input
                      id="email"
                      type="email"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      placeholder="Enter your email"
                      disabled={isPending}
                      className={field.state.meta.errors.length > 0 ? 'border-red-500' : ''}
                    />
                    {field.state.meta.errors.map((error) => (
                      <p key={error as string} className="text-red-500 text-sm">{error}</p>
                    ))}
                  </div>
                )}
              </form.Field>
            </div>

            {/* Password Field */}
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <form.Field
                name="password"
                validators={{
                  onChange: ({ value }) =>
                    !value
                      ? 'Password is required'
                      : value.length < 8
                        ? 'Password must be at least 8 characters'
                        : undefined,
                }}
              >
                {(field) => (
                  <div>
                    <Input
                      id="password"
                      type="password"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      placeholder="Enter your password"
                      disabled={isPending}
                      className={field.state.meta.errors.length > 0 ? 'border-red-500' : ''}
                    />
                    {field.state.meta.errors.map((error) => (
                      <p key={error as string} className="text-red-500 text-sm">{error}</p>
                    ))}
                  </div>
                )}
              </form.Field>
            </div>

            {/* Confirm Password Field */}
            <div className="space-y-2">
              <Label htmlFor="confirmPassword">Confirm Password</Label>
              <form.Field
                name="confirmPassword"
                validators={{
                  onChange: ({ value, fieldApi }) => {
                    const password = fieldApi.form.getFieldValue('password');
                    return !value
                      ? 'Please confirm your password'
                      : value !== password
                        ? 'Passwords do not match'
                        : undefined;
                  },
                }}
              >
                {(field) => (
                  <div>
                    <Input
                      id="confirmPassword"
                      type="password"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      placeholder="Confirm your password"
                      disabled={isPending}
                      className={field.state.meta.errors.length > 0 ? 'border-red-500' : ''}
                    />
                    {field.state.meta.errors.map((error) => (
                      <p key={error as string} className="text-red-500 text-sm">{error}</p>
                    ))}
                  </div>
                )}
              </form.Field>
            </div>

            {/* Submit Button */}
            <Button type="submit" disabled={isPending} className="w-full">
              {isPending ? 'Registering...' : 'Register'}
            </Button>

            {/* Error Message */}
            {error && (
              <p className="text-red-500 text-center text-sm">{error.message}</p>
            )}

            {/* Success Message */}
            {isSuccess && (
              <p className="text-green-500 text-center text-sm">
                Registration successful! Redirecting...
              </p>
            )}
          </form>
        </CardContent>
      </Card>
    </div>
  );
}