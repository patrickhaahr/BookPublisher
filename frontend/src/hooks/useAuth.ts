import { useEffect, useState, useCallback } from 'react';
import { checkUserRoleFromToken, isTokenExpired, refreshAccessToken } from '../lib/authUtils';

// Custom event name for auth state changes
const AUTH_STATE_CHANGE_EVENT = 'auth-state-change';

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [isRefreshing, setIsRefreshing] = useState<boolean>(false);

  const checkAuth = useCallback(async () => {
    // If already refreshing, wait
    if (isRefreshing) return;

    const accessToken = localStorage.getItem('accessToken');
    
    // Check if token needs refresh
    if (accessToken && isTokenExpired()) {
      setIsRefreshing(true);
      const refreshSuccess = await refreshAccessToken();
      setIsRefreshing(false);
      
      if (!refreshSuccess) {
        setIsAuthenticated(false);
        setUserRole(null);
        return;
      }
    }

    const tokenIsValid = !!accessToken && !isTokenExpired();
    setIsAuthenticated(tokenIsValid);

    if (tokenIsValid) {
      const role = checkUserRoleFromToken();
      setUserRole(role);
    } else {
      setUserRole(null);
    }
  }, [isRefreshing]);

  // Function to handle login
  const login = useCallback((accessToken: string, refreshToken: string) => {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    checkAuth(); // Call checkAuth to decode token and set state

    // Dispatch custom event to notify all components
    window.dispatchEvent(new CustomEvent(AUTH_STATE_CHANGE_EVENT));
  }, [checkAuth]);

  // Function to handle logout
  const logout = useCallback(() => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    setIsAuthenticated(false);
    setUserRole(null);

    // Dispatch custom event
    window.dispatchEvent(new CustomEvent(AUTH_STATE_CHANGE_EVENT));

    // Also dispatch storage event for cross-tab sync
    window.dispatchEvent(new Event('storage'));
  }, []);

  useEffect(() => {
    // Initial check
    checkAuth();

    // Set up interval to check token expiration
    const intervalId = setInterval(() => {
      if (isAuthenticated) {
        checkAuth();
      }
    }, 60000); // Check every minute

    // Listen for storage changes
    window.addEventListener('storage', checkAuth);

    // Listen for our custom auth state change event
    window.addEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);

    return () => {
      clearInterval(intervalId);
      window.removeEventListener('storage', checkAuth);
      window.removeEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);
    };
  }, [checkAuth, isAuthenticated]);

  // Expose the user role along with other auth state
  return { isAuthenticated, userRole, login, logout, isRefreshing };
}; 