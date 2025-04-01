import { jwtDecode, type JwtPayload } from 'jwt-decode';

// Standard claim type for roles in ASP.NET Core Identity
const ROLE_CLAIM_TYPE = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

// Define a custom payload interface extending JwtPayload
// Use index signature to allow accessing the claim by variable name
interface CustomJwtPayload extends JwtPayload {
  [ROLE_CLAIM_TYPE]?: string | string[];
}

/**
 * Checks if the current access token is expired or will expire soon
 * @param bufferSeconds Seconds before expiration to consider token as expired
 * @returns boolean indicating if token needs refresh
 */
export const isTokenExpired = (bufferSeconds = 60): boolean => {
  const accessToken = localStorage.getItem('accessToken');
  if (!accessToken) return true;

  try {
    const decoded = jwtDecode<JwtPayload>(accessToken);
    if (!decoded.exp) return true;

    const expirationTime = decoded.exp * 1000; // Convert to milliseconds
    const currentTime = Date.now();
    const bufferTime = bufferSeconds * 1000;

    return currentTime >= expirationTime - bufferTime;
  } catch {
    return true;
  }
};

/**
 * Validates if the refresh token exists and looks valid
 * @returns boolean indicating if refresh token appears valid
 */
export const hasValidRefreshToken = (): boolean => {
  const refreshToken = localStorage.getItem('refreshToken');
  // Basic validation - check if it exists and has a reasonable length
  return !!refreshToken && refreshToken.length > 20;
};

/**
 * Attempts to refresh the access token using the refresh token
 * @returns Promise<boolean> indicating if refresh was successful
 */
export const refreshAccessToken = async (): Promise<boolean> => {
  const refreshToken = localStorage.getItem('refreshToken');
  const userId = getUserIdFromToken();
  
  // Early return if we don't have the necessary data
  if (!refreshToken || !userId) {
    // Clean up any invalid tokens
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userId');
    return false;
  }

  try {
    const response = await fetch('http://localhost:5094/api/v1/auth/refresh-token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        userId,
        refreshToken,
      }),
    });

    if (!response.ok) {
      // If server rejects the token, clear both tokens
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('userId');
      throw new Error(`Token refresh failed: ${response.status}`);
    }

    const data = await response.json();
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('userId', data.userId);
    
    // Dispatch auth state change event
    window.dispatchEvent(new CustomEvent('auth-state-change'));
    return true;
  } catch (error) {
    console.error('Failed to refresh token:', error);
    // Ensure tokens are cleared on failure
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userId');
    window.dispatchEvent(new CustomEvent('auth-state-change'));
    return false;
  }
};

/**
 * Gets the user ID from the current access token
 * @returns string | null
 */
export const getUserIdFromToken = (): string | null => {
  // First check if we already have the userId in localStorage
  const storedUserId = localStorage.getItem('userId');
  if (storedUserId) {
    return storedUserId;
  }
  
  const accessToken = localStorage.getItem('accessToken');
  if (!accessToken) return null;

  try {
    const decoded = jwtDecode<JwtPayload>(accessToken);
    const userId = decoded.sub || null;
    
    // Store userId in localStorage for future use if we find it
    if (userId) {
      localStorage.setItem('userId', userId);
    }
    
    return userId;
  } catch {
    return null;
  }
};

/**
 * Checks the role directly from the access token stored in localStorage.
 * @returns The user's primary role (e.g., "Admin", "User") or null.
 */
export const checkUserRoleFromToken = (): string | null => {
  const accessToken = localStorage.getItem('accessToken');

  if (!accessToken) {
    return null;
  }

  try {
    const decoded = jwtDecode<CustomJwtPayload>(accessToken);
    // Access the claim using the correct constant
    const roleClaim = decoded[ROLE_CLAIM_TYPE];

    let primaryRole: string | null = null;
    if (Array.isArray(roleClaim)) {
      // Check case-insensitively if 'Admin' is present
      primaryRole = roleClaim.some(role => role.toLowerCase() === 'admin')
        ? 'Admin' // Return consistent casing
        : roleClaim[0] ?? null;
    } else if (typeof roleClaim === 'string') {
      // Check case-insensitively
      primaryRole = roleClaim.toLowerCase() === 'admin'
        ? 'Admin' // Return consistent casing
        : roleClaim;
    }

    return primaryRole;
  } catch (error) {
    console.error('Failed to decode JWT for role check:', error);
    return null;
  }
}; 