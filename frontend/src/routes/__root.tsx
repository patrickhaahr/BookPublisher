import { createRootRoute, Link, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools'
import { ThemeProvider } from '@/components/theme-provider'
import { ThemeToggle } from '@/components/theme-toggle'
import { Book, User, LayoutDashboard } from 'lucide-react'
import { useLogout } from '@/hooks/useLogout'
import { useAuth } from '@/hooks/useAuth'
import { useEffect, useState } from 'react'

export const Route = createRootRoute({
  component: () => {
    const { mutate: logout, isPending: isLoggingOut } = useLogout();
    const { isAuthenticated, hasMsalAccount } = useAuth();
    const [forceUpdate, setForceUpdate] = useState(0);

    // Add a listener to force re-render when auth state changes
    useEffect(() => {
      const handleAuthChange = () => {
        setForceUpdate(prev => prev + 1);
      };

      // Listen for both storage and custom auth events
      window.addEventListener('storage', handleAuthChange);
      window.addEventListener('auth-state-change', handleAuthChange);

      return () => {
        window.removeEventListener('storage', handleAuthChange);
        window.removeEventListener('auth-state-change', handleAuthChange);
      };
    }, []);

    // Check if any authentication is active
    const anyAuthActive = isAuthenticated || hasMsalAccount();

    console.log('Root component rendering, authenticated:', isAuthenticated, 'Entra ID active:', hasMsalAccount(), 'update:', forceUpdate);

    // Handle logout for both authentication methods
    const handleLogout = () => {
      if (isLoggingOut) return; // Prevent multiple clicks
      logout();
    };

    return (
      <ThemeProvider defaultTheme="light" storageKey="book-publisher-theme">
        <div className="relative min-h-screen flex flex-col bg-background text-foreground">
          <header className="sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
            <div className="container mx-auto px-4 sm:px-6 lg:px-8">
              <div className="flex h-14 items-center justify-between gap-4">
                <div className="flex items-center gap-6">
                  <Link to="/" className="flex items-center space-x-2">
                    <Book className="h-5 w-5" />
                    <span className="font-bold">Book Publisher</span>
                  </Link>
                  <nav className="hidden sm:flex items-center space-x-6 text-sm font-medium">
                    <Link
                      to="/"
                      className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                    >
                      Home
                    </Link>
                    <Link
                      to="/books"
                      className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                    >
                      Books
                    </Link>
                    <Link
                      to="/about"
                      className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                    >
                      About
                    </Link>
                  </nav>
                </div>
                
                <div className="flex items-center gap-4">
                  <nav className="flex items-center space-x-6 text-sm font-medium">
                    {!anyAuthActive ? (
                      <>
                        <Link
                          to="/auth/login"
                          className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                        >
                          Login
                        </Link>
                        <Link 
                          to="/auth/register" 
                          className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                        >
                          Register
                        </Link>
                      </>
                    ) : (
                      <button 
                        onClick={handleLogout}
                        disabled={isLoggingOut}
                        className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                      >
                        {isLoggingOut ? 'Logging out...' : 'Logout'}
                      </button>
                    )}
                  </nav>
                  <ThemeToggle />
                  {anyAuthActive && (
                    <>
                      <Link
                        to="/profile"
                        className="transition-colors hover:text-foreground/80 text-foreground/60"
                      >
                        <User className="h-5 w-5" />
                      </Link>
                      <Link
                        to="/admin"
                        className="transition-colors hover:text-foreground/80 text-foreground/60"
                        aria-label="Admin Dashboard"
                      >
                        <LayoutDashboard className="h-5 w-5" />
                      </Link>
                    </>
                  )}
                </div>
              </div>
            </div>
          </header>
          <main className="flex-1">
            <div className="container mx-auto px-4 sm:px-6 lg:px-8 py-6">
              <Outlet />
            </div>
          </main>
          <footer className="border-t">
            <div className="container mx-auto px-4 sm:px-6 lg:px-8">
              <div className="flex h-14 items-center justify-between">
                <p className="text-sm text-muted-foreground">
                  ©patrickhaahr
                </p>
              </div>
            </div>
          </footer>
          {import.meta.env.DEV && <TanStackRouterDevtools />}
        </div>
      </ThemeProvider>
    )
  },
})