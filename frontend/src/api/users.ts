import type { UserProfile } from '@/types';

const API_BASE_URL = 'http://localhost:5094/api/v1';

/**
 * Fetches user profile by user ID
 */
export const getUserProfile = async (userId: string): Promise<UserProfile> => {
  const token = localStorage.getItem('accessToken');
  
  if (!token) {
    throw new Error('Authentication token not found');
  }
  
  const response = await fetch(`${API_BASE_URL}/users/${userId}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });

  if (!response.ok) {
    throw new Error('Failed to fetch user profile');
  }

  return response.json();
};
