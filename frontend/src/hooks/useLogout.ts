import { useMutation } from '@tanstack/react-query';
import { useNavigate } from '@tanstack/react-router';
import { useAuth } from './useAuth';

const logoutUser = async (): Promise<void> => {
  const accessToken = localStorage.getItem('accessToken');
  
  const response = await fetch('http://localhost:5094/api/v1/auth/logout', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${accessToken}`,
    },
  });

  if (!response.ok) {
    throw new Error('Logout failed');
  }

  // Clear tokens and trigger storage event
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  // Dispatch storage event for cross-tab sync
  window.dispatchEvent(new Event('storage'));
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
      logout();
      navigate({ to: '/' });
    },
  });
}; 