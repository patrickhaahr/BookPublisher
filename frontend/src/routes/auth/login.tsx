import { useForm } from '@tanstack/react-form';
import { useMutation } from '@tanstack/react-query';
import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useAuth } from '@/hooks/useAuth';
import { useMsal } from '@/hooks/authConfig';

// Import API function
import { loginUser } from '@/api';

export const Route = createFileRoute('/auth/login')({
  component: Login,
});

function Login() {
  const navigate = useNavigate();
  const { login, isAuthenticated, logout } = useAuth();
  const auth = useMsal();
  
  // TanStack Form setup
  const form = useForm({
    defaultValues: {
      email: '',
      password: '',
    },
    onSubmit: async ({ value }) => {
      // If already logged in, log out first
      if (isAuthenticated) {
        logout();
      }
      
      // Clear any existing Entra ID state
      auth.clearMsalState();
      
      // Now proceed with new login
      mutate(value);
    },
  });

  // Handle Microsoft authentication
  const handleMsalLogin = async () => {
    try {
      // If already logged in, log out first
      if (isAuthenticated) {
        logout();
      }
      
      await auth.msalInstance.initialize();
      await auth.msalInstance.loginPopup();
      const myAccounts = auth.msalInstance.getAllAccounts();
      auth.account = myAccounts[0];

      const response = await auth.msalInstance.acquireTokenSilent({
        account: auth.account,
        scopes: [`api://${import.meta.env.VITE_AZURE_CLIENT_ID}/API.Read`],
      });
      
      auth.token = response.accessToken;
      
      // Use the same login flow as form submission
      login(response.accessToken, response.idToken, response.account.localAccountId);
      navigate({ to: '/' });
    } catch (error) {
      console.error('MSAL login error:', error);
    }
  };

  // TanStack Query mutation for login
  const { mutate, isPending, error, isSuccess } = useMutation({
    mutationFn: loginUser,
    onSuccess: (data) => {
      // Handle successful login
      console.log('Login successful:', data);
      // Use the login function from useAuth instead of directly setting localStorage
      login(data.accessToken, data.refreshToken, data.userId);
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
          {isAuthenticated && (
            <p className="text-center text-amber-500 text-sm mt-2">
              Note: Logging in will replace your current authentication.
            </p>
          )}
        </CardHeader>
        <CardContent>
          <div className="mb-4">
            <Button 
              onClick={handleMsalLogin} 
              variant="outline" 
              className="w-full"
              disabled={isPending}
            >
              Sign in with Microsoft
            </Button>
          </div>
          
          <div className="relative my-4">
            <div className="absolute inset-0 flex items-center">
              <span className="w-full border-t"></span>
            </div>
            <div className="relative flex justify-center text-xs uppercase">
              <span className="bg-background px-2 text-muted-foreground">Or continue with</span>
            </div>
          </div>
          
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
