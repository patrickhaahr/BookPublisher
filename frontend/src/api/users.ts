import type { 
  UsersResponse, 
  User,
} from '../types/user';

const API_BASE_URL = 'http://localhost:5094/api/v1';

/**
 * Fetches the current user's profile
 */
export const getUserProfile = async (userId: string): Promise<User> => {
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
  
  return await response.json();
};

/**
 * Fetches users with pagination
 */
export const getUsers = async (
  page: number,
  pageSize: number,
  searchQuery?: string
): Promise<UsersResponse> => {
  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
    ...(searchQuery && { search: searchQuery }),
  });

  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/users?${queryParams}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  if (!response.ok) {
    throw new Error('Failed to fetch users');
  }
  
  return await response.json() as UsersResponse;
};

/**
 * Deletes a user
 */
export const deleteUser = async (userId: string): Promise<void> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/users/${userId}`, {
    method: 'DELETE',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });

  if (!response.ok) {
    let errorData;
    try {
      errorData = await response.json();
    } catch (e) {
      throw new Error(`HTTP error ${response.status}: ${response.statusText}`);
    }
    throw new Error(errorData?.title || errorData?.message || `HTTP error ${response.status}`);
  }
};

/**
 * Updates a user's role
 */
export const updateUserRole = async (userId: string, role: string): Promise<User> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/users/${userId}/role`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({ role }),
  });

  if (!response.ok) {
    let errorData;
    try {
      errorData = await response.json();
    } catch (e) {
      throw new Error(`HTTP error ${response.status}: ${response.statusText}`);
    }
    throw new Error(errorData?.title || errorData?.message || `HTTP error ${response.status}`);
  }

  return await response.json();
};
