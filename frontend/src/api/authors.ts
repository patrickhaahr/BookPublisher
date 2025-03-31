import type { AuthorsResponse, Author } from '../types/author';

const API_BASE_URL = 'http://localhost:5094/api/v1';

/**
 * Fetches a specific author by ID
 */
export const getAuthor = async (authorId: string): Promise<Author> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/authors/${authorId}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  if (!response.ok) {
    throw new Error('Failed to fetch author');
  }
  
  return await response.json() as Author;
};

/**
 * Updates an existing author
 */
export const updateAuthor = async ({
  authorId,
  authorData
}: {
  authorId: string,
  authorData: {
    firstname: string;
    lastname: string;
    email: string;
    phone: string;
    royaltyrate: number;
  }
}): Promise<Author> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/authors/${authorId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(authorData),
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

/**
 * Fetches authors with pagination
 */
export const getAuthors = async (
  page: number,
  pageSize: number,
  searchQuery?: string
): Promise<AuthorsResponse> => {
  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
    ...(searchQuery && { search: searchQuery }),
  });

  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/authors?${queryParams}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  if (!response.ok) {
    throw new Error('Failed to fetch authors');
  }
  
  return await response.json() as AuthorsResponse;
};

/**
 * Creates a new author
 */
export const createAuthor = async (authorData: {
  firstname: string;
  lastname: string;
  email: string;
  phone: string;
  royaltyrate: number;
}): Promise<Author> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/authors`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(authorData),
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

/**
 * Deletes an author
 */
export const deleteAuthor = async (authorId: string): Promise<void> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/authors/${authorId}`, {
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