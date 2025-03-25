import { useEffect, useState, useCallback } from 'react';

// Custom event name for auth state changes
const AUTH_STATE_CHANGE_EVENT = 'auth-state-change';

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
    // Initialize with current auth state
    return !!localStorage.getItem('accessToken');
  });

  const checkAuth = useCallback(() => {
    const accessToken = localStorage.getItem('accessToken');
    setIsAuthenticated(!!accessToken);
  }, []);

  // Function to handle login (to be called after successful API login)
  const login = useCallback((accessToken: string, refreshToken: string) => {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    setIsAuthenticated(true);
    
    // Dispatch custom event to notify all components of auth state change
    window.dispatchEvent(new CustomEvent(AUTH_STATE_CHANGE_EVENT));
  }, []);

  // Function to handle logout
  const logout = useCallback(() => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    setIsAuthenticated(false);
    
    // Dispatch custom event to notify all components of auth state change
    window.dispatchEvent(new CustomEvent(AUTH_STATE_CHANGE_EVENT));
    
    // Also dispatch storage event for cross-tab sync
    window.dispatchEvent(new Event('storage'));
  }, []);

  useEffect(() => {
    // Initial check
    checkAuth();

    // Listen for storage changes (in case other tabs modify auth state)
    window.addEventListener('storage', checkAuth);
    
    // Listen for our custom auth state change event
    window.addEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);

    return () => {
      window.removeEventListener('storage', checkAuth);
      window.removeEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);
    };
  }, [checkAuth]);

  return { isAuthenticated, login, logout };
}; 