import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Link } from '@tanstack/react-router';
import { ShieldAlert } from 'lucide-react';

interface NotAuthenticatedCardProps {
  title?: string;
  description?: string;
  showLoginLink?: boolean;
}

export function NotAuthenticatedCard({
  title = 'Not Authenticated',
  description = 'Please log in to view this page.',
  showLoginLink = true,
}: NotAuthenticatedCardProps) {
  return (
    <Card className="w-full max-w-md mx-auto mt-8 border-destructive/50">
      <CardHeader className="text-center">
        <div className="mx-auto bg-destructive/10 rounded-full p-3 w-fit mb-3">
            <ShieldAlert className="w-8 h-8 text-destructive" />
        </div>
        <CardTitle className="text-destructive">{title}</CardTitle>
        <CardDescription>{description}</CardDescription>
      </CardHeader>
      {showLoginLink && (
        <CardContent className="text-center">
          <Link
            to="/auth/login"
            className="text-sm font-medium text-primary hover:underline"
            // Optionally preserve the redirect path if needed, though often not necessary here
            // search={{ redirect: window.location.pathname + window.location.search }}
          >
            Go to Login Page
          </Link>
        </CardContent>
      )}
    </Card>
  );
} 