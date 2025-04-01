import { useEffect, useState, useCallback, useRef } from 'react';
import { 
  checkUserRoleFromToken, 
  isTokenExpired, 
  refreshAccessToken,
  hasValidRefreshToken
} from '../lib/authUtils';
import { useMsal } from './authConfig';

// Custom event name for auth state changes
const AUTH_STATE_CHANGE_EVENT = 'auth-state-change';

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [isRefreshing, setIsRefreshing] = useState<boolean>(false);
  const auth = useMsal();
  
  // Use a ref to track the last refresh attempt time to prevent spamming
  const lastRefreshAttempt = useRef<number>(0);
  // Minimum time between refresh attempts in milliseconds (15 seconds)
  const MIN_REFRESH_INTERVAL = 15000;

  const checkAuth = useCallback(async () => {
    // If already refreshing, wait
    if (isRefreshing) return;

    const accessToken = localStorage.getItem('accessToken');
    const currentTime = Date.now();
    
    // Check if token needs refresh
    if (accessToken && isTokenExpired()) {
      // Check if we should attempt a refresh based on time since last attempt
      if (currentTime - lastRefreshAttempt.current < MIN_REFRESH_INTERVAL) {
        console.log('Skipping refresh - too soon since last attempt');
        return;
      }
      
      // Check if we have what looks like a valid refresh token before attempting
      if (!hasValidRefreshToken()) {
        setIsAuthenticated(false);
        setUserRole(null);
        return;
      }

      // Update last refresh attempt time
      lastRefreshAttempt.current = currentTime;
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
    
    // Reset last refresh time on manual login
    lastRefreshAttempt.current = 0;
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
  }, []);

  // Function to check if user is authenticated via MSAL
  const hasMsalAccount = useCallback(() => {
    return auth.msalInstance.getAllAccounts().length > 0;
  }, [auth.msalInstance]);

  useEffect(() => {
    // Initial check
    checkAuth();

    // Set up interval to check token expiration (every 5 minutes)
    // This is less frequent to avoid constant checking
    const intervalId = setInterval(() => {
      if (isAuthenticated) {
        checkAuth();
      }
    }, 300000); // Check every 5 minutes instead of every minute

    // Listen for storage changes
    const handleStorageChange = (event: StorageEvent) => {
      if (event.key === 'accessToken' || event.key === 'refreshToken') {
        checkAuth();
      }
    };
    
    window.addEventListener('storage', handleStorageChange);

    // Listen for our custom auth state change event
    window.addEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);

    return () => {
      clearInterval(intervalId);
      window.removeEventListener('storage', handleStorageChange);
      window.removeEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);
    };
  }, [checkAuth, isAuthenticated]);

  // Expose the user role along with other auth state
  return { 
    isAuthenticated, 
    userRole, 
    login, 
    logout, 
    isRefreshing,
    hasMsalAccount
  };
}; 