import { useMsal } from '@/hooks/authConfig'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'

interface EntraIdProfileProps {
  className?: string;
  showCard?: boolean;
}

export function EntraIdProfile({ className = '', showCard = true }: EntraIdProfileProps) {
  const auth = useMsal();
  const account = auth.msalInstance.getAllAccounts()[0];
  
  const content = account ? (
    <div className="space-y-4">
      <div>
        <h3 className="font-medium">Name</h3>
        <p>{account.name}</p>
      </div>
      <div>
        <h3 className="font-medium">Username</h3>
        <p>{account.username}</p>
      </div>
    </div>
  ) : (
    <p>Please log in with Microsoft Entra ID to view your profile.</p>
  );

  if (!showCard) {
    return content;
  }

  return (
    <div className={`container mx-auto py-8 ${className}`}>
      <Card className="max-w-md mx-auto">
        <CardHeader>
          <CardTitle>Entra ID Profile</CardTitle>
        </CardHeader>
        <CardContent>
          {content}
        </CardContent>
      </Card>
    </div>
  );
} 