import type { 
  BooksResponse, 
  BookDetails,
  CreateBookApiPayload,
  CreateBookApiResponse
} from '@/types';

const API_BASE_URL = 'http://localhost:5094/api/v1';

/**
 * Fetches books with optional filtering
 */
export const getBooks = async (
  page: number,
  pageSize: number,
  title?: string,
  author?: string,
  genre?: string,
  medium?: string,
  year?: string
): Promise<BooksResponse> => {
  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
    ...(title && { title }),
    ...(author && { author }),
    ...(genre && { genre }),
    ...(medium && { medium }),
    ...(year && { year }),
  });

  console.log('Fetching books with params:', queryParams.toString());
  const response = await fetch(`${API_BASE_URL}/books?${queryParams}`);
  
  if (!response.ok) {
    throw new Error('Failed to fetch books');
  }
  
  return await response.json() as BooksResponse;
};

/**
 * Fetches a single book by ID
 */
export const getBookById = async (bookId: string): Promise<BookDetails> => {
  const response = await fetch(`${API_BASE_URL}/books/${bookId}`);
  
  if (!response.ok) {
    throw new Error('Failed to fetch book details');
  }
  
  return await response.json() as BookDetails;
};

/**
 * Creates a new book
 */
export const createBook = async (payload: CreateBookApiPayload): Promise<CreateBookApiResponse> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/books`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(payload),
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
 * Updates an existing book
 */
export const updateBook = async (bookId: string, payload: any): Promise<BookDetails> => {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/books/${bookId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(payload),
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
 * Converts a File to base64 string
 */
export const fileToBase64 = (file: File): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      if (typeof reader.result === 'string') {
        // Remove the "data:*/*;base64," prefix
        resolve(reader.result.split(',')[1]);
      } else {
        reject(new Error('Failed to read file as base64 string'));
      }
    };
    reader.onerror = (error) => reject(error);
  });
};
