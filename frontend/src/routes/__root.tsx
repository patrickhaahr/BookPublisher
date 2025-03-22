import { createRootRoute, Link, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools'
import { ThemeProvider } from '@/components/theme-provider'
import { ThemeToggle } from '@/components/ui/theme-toggle'
import { SearchBar } from '@/components/search-bar'
import { Book } from 'lucide-react'

export const Route = createRootRoute({
  component: () => (
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
                    to="/about"
                    className="transition-colors hover:text-foreground/80 text-foreground/60 [&.active]:text-foreground"
                  >
                    About
                  </Link>
                </nav>
              </div>
              <div className="flex-1 max-w-sm hidden md:block">
                <SearchBar />
              </div>
              <div className="flex items-center gap-4">
                <div className="md:hidden">
                  <SearchBar />
                </div>
                <ThemeToggle />
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
                Built with modern web technologies
              </p>
            </div>
          </div>
        </footer>
        {import.meta.env.DEV && <TanStackRouterDevtools />}
      </div>
    </ThemeProvider>
  ),
})