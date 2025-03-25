import { StrictMode } from 'react'
import ReactDOM from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { QueryClientProvider, QueryClient } from '@tanstack/react-query'
import { AuthProvider } from './contexts/AuthContext'

// Import the generated route tree
import { routeTree } from './routeTree.gen'

// Create a new router instance
const router = createRouter({ 
  routeTree,
  // Turn off default preloading during development
  defaultPreload: false,
})

// Register the router instance for type safety
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

// Make sure the router is ready before rendering
await router.load()

// Render the app
const rootElement = document.getElementById('root')
if (!rootElement) throw new Error('Root element not found')

const root = ReactDOM.createRoot(rootElement)
const queryClient = new QueryClient()
root.render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <RouterProvider router={router} />
      </AuthProvider>
    </QueryClientProvider>
  </StrictMode>
)