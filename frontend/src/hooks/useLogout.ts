import { useMutation } from '@tanstack/react-query';
import { useNavigate } from '@tanstack/react-router';
import { useAuth } from './useAuth';
import { useMsal } from './authConfig';

const logoutUser = async (): Promise<void> => {
  const accessToken = localStorage.getItem('accessToken');
  
  // Skip the API call if there's no token
  if (!accessToken) {
    return;
  }

  try {
    const response = await fetch('http://localhost:5094/api/v1/auth/logout', {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
      },
    });

    if (!response.ok) {
      // Still proceed with local logout even if server logout fails
      console.warn(`Logout API failed with status: ${response.status}`);
    }
  } catch (error) {
    console.warn('Error during logout API call:', error);
    // Continue with local logout even if API call fails
  } finally {
    // Always clear tokens
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    // Dispatch custom event for auth state change
    window.dispatchEvent(new CustomEvent('auth-state-change'));
  }
};

export const useLogout = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();
  const msalAuth = useMsal();
  
  // Check if we have an MSAL session active
  const hasMsalAccount = msalAuth.msalInstance.getAllAccounts().length > 0;

  return useMutation({
    mutationFn: async () => {
      try {
        // First logout from local JWT
        await logoutUser();
        
        // Then clear Entra ID state with specific handling
        if (hasMsalAccount) {
          console.log("MSAL account detected, clearing MSAL state");
          // First clear local MSAL state
          msalAuth.account = null;
          msalAuth.token = "";
          
          // Then trigger MSAL logout
          await msalAuth.msalInstance.logoutPopup({
            mainWindowRedirectUri: window.location.origin,
          }).catch(e => {
            console.warn("Failed to logout popup, trying alternative:", e);
            // Try cache clearing as fallback
            msalAuth.msalInstance.clearCache();
          });
        }
      } catch (error) {
        console.error("Error during logout process:", error);
        // Make sure local state is cleared even on error
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
      }
    },
    onSuccess: () => {
      // Use the logout function from useAuth for local state cleanup
      logout();
      // Force trigger the auth state change event again to ensure UI updates
      window.dispatchEvent(new CustomEvent('auth-state-change'));
      navigate({ to: '/' });
    },
    onError: (error) => {
      console.error('Logout error:', error);
      // Still perform local logout
      logout();
      // Force trigger the auth state change event again
      window.dispatchEvent(new CustomEvent('auth-state-change'));
      navigate({ to: '/' });
    },
  });
}; 