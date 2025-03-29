import { useEffect, useState, useCallback } from 'react';
import { jwtDecode, type JwtPayload } from 'jwt-decode';

// Standard claim type for roles in ASP.NET Core Identity
const ROLE_CLAIM_TYPE = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

// Define a custom payload interface extending JwtPayload
interface CustomJwtPayload extends JwtPayload {
  [ROLE_CLAIM_TYPE]?: string | string[];
}

// Custom event name for auth state changes
const AUTH_STATE_CHANGE_EVENT = 'auth-state-change';

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [userRole, setUserRole] = useState<string | null>(null);

  const checkAuth = useCallback(() => {
    const accessToken = localStorage.getItem('accessToken');
    const tokenIsValid = !!accessToken;
    setIsAuthenticated(tokenIsValid);

    if (tokenIsValid) {
      try {
        const decoded = jwtDecode<CustomJwtPayload>(accessToken);
        const roleClaim = decoded[ROLE_CLAIM_TYPE]; // Use correct claim name
        let primaryRole: string | null = null;
        if (Array.isArray(roleClaim)) {
          // Check case-insensitively
          primaryRole = roleClaim.some(role => role.toLowerCase() === 'admin')
            ? 'Admin'
            : roleClaim[0] ?? null;
        } else if (typeof roleClaim === 'string') {
          // Check case-insensitively
          primaryRole = roleClaim.toLowerCase() === 'admin'
            ? 'Admin'
            : roleClaim;
        }
        setUserRole(primaryRole);
      } catch (error) {
        console.error('Failed to decode JWT in useAuth:', error);
        setUserRole(null);
      }
    } else {
      setUserRole(null);
    }
  }, []);

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

    // Listen for storage changes
    window.addEventListener('storage', checkAuth);

    // Listen for our custom auth state change event
    window.addEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);

    return () => {
      window.removeEventListener('storage', checkAuth);
      window.removeEventListener(AUTH_STATE_CHANGE_EVENT, checkAuth);
    };
  }, [checkAuth]);

  // Expose the user role along with other auth state
  return { isAuthenticated, userRole, login, logout };
}; 