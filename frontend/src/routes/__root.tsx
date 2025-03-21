import { createRootRoute, Link, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools'

export const Route = createRootRoute({
  component: () => (
    <div className="min-h-screen flex flex-col">
      <header className="bg-slate-100 p-4 shadow-sm">
        <nav className="flex gap-4">
          <Link to="/" className="[&.active]:font-bold [&.active]:text-blue-600">
            Home
          </Link>
          <Link to="/about" className="[&.active]:font-bold [&.active]:text-blue-600">
            About
          </Link>
        </nav>
      </header>
      <main className="flex-1 p-4">
        <Outlet />
      </main>
      {import.meta.env.DEV && <TanStackRouterDevtools />}
    </div>
  ),
})