import { createFileRoute } from '@tanstack/react-router'
import { useAuth } from '../../hooks/useAuth'
import { useQuery } from '@tanstack/react-query'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Skeleton } from '../../components/ui/skeleton'
import { Badge } from '../../components/ui/badge'

interface UserProfile {
  userId: string
  username: string
  email: string
  role: string
}

// API function to get user profile
const getUserProfile = async (userId: string): Promise<UserProfile> => {
  const response = await fetch(`http://localhost:5094/api/v1/users/${userId}`, {
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
    }
  })

  if (!response.ok) {
    throw new Error('Failed to fetch user profile')
  }

  return response.json()
}

export const Route = createFileRoute('/profile/')({
  component: RouteComponent,
})

function RouteComponent() {
  const { isAuthenticated } = useAuth()
  
  // Get userId from JWT token
  const token = localStorage.getItem('accessToken')
  const userId = token ? JSON.parse(atob(token.split('.')[1])).sub : null

  const { data: profile, isLoading, error } = useQuery<UserProfile>({
    queryKey: ['profile', userId],
    queryFn: () => getUserProfile(userId),
    enabled: !!userId && isAuthenticated,
    staleTime: 1000 * 60 * 5, // Store cached data for 5 minutes
    gcTime: 1000 * 60 * 5, // Remove unused cached data after 5 minutes
  })

  if (!isAuthenticated) {
    return (
      <Card className="max-w-md mx-auto mt-8">
        <CardHeader className="text-center">
          <CardTitle className="text-red-500">Not Authenticated</CardTitle>
          <CardDescription>Please log in to view your profile</CardDescription>
        </CardHeader>
      </Card>
    )
  }

  if (isLoading || !profile) {
    return (
      <Card className="max-w-md mx-auto mt-8">
        <CardHeader className="space-y-4">
          <Skeleton className="h-8 w-[200px] mx-auto" />
          <Skeleton className="h-4 w-[150px] mx-auto" />
        </CardHeader>
        <CardContent className="space-y-4">
          <Skeleton className="h-4 w-[300px]" />
          <Skeleton className="h-4 w-[250px]" />
        </CardContent>
      </Card>
    )
  }

  if (error) {
    return (
      <Card className="max-w-md mx-auto mt-8">
        <CardHeader className="text-center">
          <CardTitle className="text-red-500">Error</CardTitle>
          <CardDescription>{error instanceof Error ? error.message : 'Failed to load profile'}</CardDescription>
        </CardHeader>
      </Card>
    )
  }

  return (
    <Card className="max-w-md mx-auto mt-8">
      <CardHeader className="text-center border-b pb-4">
        <CardTitle className="text-3xl font-bold bg-gradient-to-r from-primary to-primary/70 bg-clip-text text-transparent">
          {profile.username}
        </CardTitle>
        <div className="mt-2">
          <Badge variant="secondary" className="text-sm">{profile.role}</Badge>
        </div>
      </CardHeader>
      <CardContent className="space-y-6 pt-6">
        <div className="flex flex-col space-y-2">
          <p className="text-sm font-medium text-muted-foreground">Email</p>
          <p className="text-sm font-medium">{profile.email}</p>
        </div>
        <div className="flex flex-col space-y-2">
          <p className="text-sm font-medium text-muted-foreground">User ID</p>
          <p className="text-sm font-mono bg-muted p-2 rounded-md overflow-x-auto">
            {profile.userId}
          </p>
        </div>
      </CardContent>
    </Card>
  )
}
