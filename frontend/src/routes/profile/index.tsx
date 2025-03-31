import { createFileRoute } from '@tanstack/react-router'
import { useAuth } from '@/hooks/useAuth'
import { useQuery } from '@tanstack/react-query'
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { NotAuthenticatedCard } from '@/components/auth/not-authenticated-card'
import { getUserProfile } from '@/api'
import { UserProfile } from '@/types/user'
import { EditIcon } from 'lucide-react'

export const Route = createFileRoute('/profile/')({
  component: Profile,
})

function Profile() {
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
    return <NotAuthenticatedCard description="Please log in to view your profile." />;
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
      <CardFooter className="pt-2 pb-6">
        <Button 
          variant="outline" 
          className="w-full flex items-center justify-center" 
          asChild
        >
          <a href={`/profile/edit/${profile.userId}`}>
            <EditIcon className="w-4 h-4 mr-2" />
            Edit Profile
          </a>
        </Button>
      </CardFooter>
    </Card>
  )
}
