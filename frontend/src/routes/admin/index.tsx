import { createFileRoute, redirect } from '@tanstack/react-router'
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { FilePlusIcon, PencilIcon, TrashIcon, ViewIcon } from 'lucide-react' // Example icons
import { checkUserRoleFromToken } from '@/lib/authUtils'; // Import the helper

export const Route = createFileRoute('/admin/')({
  beforeLoad: async ({ location }) => {
    const userRole = checkUserRoleFromToken(); // Use the static helper
    const isAdmin = userRole === 'Admin';

    if (!isAdmin) {
      // Redirect non-admins to the login page
      // Assuming your login route is defined at '/auth/login'
      throw redirect({
        to: '/auth/login', // Correct path to your login route
        search: {
          // Pass the original intended location as 'redirect' search param
          // Ensure your login page component can handle this param
          redirect: location.pathname + location.search,
        },
      });
    }
    // No need to return anything if the check passes
  },
  component: AdminDashboard,
})

function AdminDashboard() {
  return (
    <div className="container mx-auto p-4 md:p-8">
      <h1 className="text-3xl font-bold mb-6">Admin Dashboard</h1>
      <p className="text-muted-foreground mb-8">
        Overview and management tools for site content and users.
      </p>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {/* Books Management Card */}
        <Card>
          <CardHeader>
            <CardTitle>Books Management</CardTitle>
            <CardDescription>Create, view, update, and delete book entries.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-2">
            <p className="text-sm text-muted-foreground flex items-center"><FilePlusIcon className="w-4 h-4 mr-2" /> Create new books</p>
            <p className="text-sm text-muted-foreground flex items-center"><PencilIcon className="w-4 h-4 mr-2" /> Update existing book details</p>
            <p className="text-sm text-muted-foreground flex items-center"><TrashIcon className="w-4 h-4 mr-2" /> Delete books</p>
            <p className="text-sm text-muted-foreground flex items-center"><ViewIcon className="w-4 h-4 mr-2" /> View all books</p>
          </CardContent>
          <CardFooter className="flex gap-2">
            {/* These buttons will later link to specific routes */}
            <Button size="sm" disabled> {/* Remove 'disabled' when implementing links */}
              <FilePlusIcon className="w-4 h-4 mr-2" /> Create Book
            </Button>
            <Button size="sm" variant="outline" disabled> {/* Remove 'disabled' when implementing links */}
              Manage Books
            </Button>
          </CardFooter>
        </Card>

        {/* Authors Management Card (Placeholder Structure) */}
        <Card>
          <CardHeader>
            <CardTitle>Authors Management</CardTitle>
            <CardDescription>Manage author profiles and associated books.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-2">
             <p className="text-sm text-muted-foreground">CRUD operations for authors.</p>
             {/* Add more specific action descriptions later */}
          </CardContent>
          <CardFooter className="flex gap-2">
            <Button size="sm" disabled>Create Author</Button>
            <Button size="sm" variant="outline" disabled>Manage Authors</Button>
          </CardFooter>
        </Card>

        {/* Artists Management Card (Placeholder Structure) */}
        <Card>
          <CardHeader>
            <CardTitle>Artists Management</CardTitle>
            <CardDescription>Manage artist profiles and associated covers.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-2">
             <p className="text-sm text-muted-foreground">CRUD operations for artists.</p>
             {/* Add more specific action descriptions later */}
          </CardContent>
          <CardFooter className="flex gap-2">
            <Button size="sm" disabled>Create Artist</Button>
            <Button size="sm" variant="outline" disabled>Manage Artists</Button>
          </CardFooter>
        </Card>

        {/* Users Management Card (Placeholder Structure) */}
         <Card>
          <CardHeader>
            <CardTitle>Users Management</CardTitle>
            <CardDescription>View and manage user accounts and roles.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-2">
             <p className="text-sm text-muted-foreground">View users, manage roles (future).</p>
             {/* Add more specific action descriptions later */}
          </CardContent>
          <CardFooter className="flex gap-2">
            <Button size="sm" variant="outline" disabled>Manage Users</Button>
          </CardFooter>
        </Card>

        {/* Add more cards for other sections like Genres, Covers etc. if needed */}

      </div>
    </div>
  )
}

// No interfaces needed for this specific component yet
