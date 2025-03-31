import type { ArtistsResponse, Artist } from '../types/artist';

const API_BASE_URL = 'http://localhost:5094/api/v1';

/**
 * Fetches artists with pagination
 */
export const getArtists = async (
  page: number,
  pageSize: number,
  searchQuery?: string
): Promise<ArtistsResponse> => {
  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
    ...(searchQuery && { search: searchQuery }),
  });

  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/artists?${queryParams}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  if (!response.ok) {
    throw new Error('Failed to fetch artists');
  }
  
  return await response.json() as ArtistsResponse;
};

/**
 * Creates a new artist
 */
export const createArtist = async (artistData: {
  firstname: string;
  lastname: string;
  email: string;
  phone: string;
  portfoliourl: string;
}): Promise<Artist> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/artists`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(artistData),
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
 * Deletes an artist
 */
export const deleteArtist = async (artistId: string): Promise<void> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/artists/${artistId}`, {
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