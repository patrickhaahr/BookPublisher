'use client'

import { useForm } from '@tanstack/react-form';
import { useMutation } from '@tanstack/react-query';
import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useAuth } from '@/hooks/useAuth';

export const Route = createFileRoute('/auth/login')({
  component: Login,
});

// Type for the API response
interface LoginResponse {
  userId: string;
  username: string;
  accessToken: string;
  refreshToken: string;
}

// API function to login
const loginUser = async (data: {
  email: string;
  password: string;
}): Promise<LoginResponse> => {
  const response = await fetch('http://localhost:5094/api/v1/auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message || 'Login failed');
  }

  return await response.json();
};

function Login() {
  const navigate = useNavigate();
  const { login } = useAuth();

  // TanStack Form setup
  const form = useForm({
    defaultValues: {
      email: '',
      password: '',
    },
    onSubmit: async ({ value }) => {
      // Trigger the mutation on form submission
      mutate(value);
    },
  });

  // TanStack Query mutation for login
  const { mutate, isPending, error, isSuccess } = useMutation({
    mutationFn: loginUser,
    onSuccess: (data) => {
      // Handle successful login
      console.log('Login successful:', data);
      // Use the login function from useAuth instead of directly setting localStorage
      login(data.accessToken, data.refreshToken);
      // Redirect to dashboard
      navigate({ to: '/' });
    },
    onError: (err) => {
      console.error('Login error:', err.message);
    },
  });

  return (
    <div className="min-h-auto flex items-center justify-center py-12">
      <Card className="w-full max-w-md mx-auto">
        <CardHeader>
          <CardTitle className="text-2xl font-bold text-center">Login</CardTitle>
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

            {/* Submit Button */}
            <Button type="submit" disabled={isPending} className="w-full">
              {isPending ? 'Logging in...' : 'Login'}
            </Button>

            {/* Error Message */}
            {error && (
              <p className="text-red-500 text-center text-sm">{error.message}</p>
            )}

            {/* Success Message */}
            {isSuccess && (
              <p className="text-green-500 text-center text-sm">
                Login successful! Redirecting...
              </p>
            )}
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
