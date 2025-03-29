import { jwtDecode, type JwtPayload } from 'jwt-decode';

// Standard claim type for roles in ASP.NET Core Identity
const ROLE_CLAIM_TYPE = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

// Define a custom payload interface extending JwtPayload
// Use index signature to allow accessing the claim by variable name
interface CustomJwtPayload extends JwtPayload {
  [ROLE_CLAIM_TYPE]?: string | string[];
}

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
    console.log("Decoded Role Claim:", roleClaim); // Add logging

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

    console.log("Determined Primary Role:", primaryRole); // Add logging
    return primaryRole;
  } catch (error) {
    console.error('Failed to decode JWT for role check:', error);
    return null;
  }
}; 