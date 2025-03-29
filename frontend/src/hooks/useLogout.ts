import { useMutation } from '@tanstack/react-query';
import { useNavigate } from '@tanstack/react-router';
import { useAuth } from './useAuth';

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

  return useMutation({
    mutationFn: logoutUser,
    onSuccess: () => {
      logout();
      navigate({ to: '/' });
    },
    onError: (error) => {
      console.error('Logout error:', error);
      // Still perform local logout
      logout();
      navigate({ to: '/' });
    },
  });
}; 