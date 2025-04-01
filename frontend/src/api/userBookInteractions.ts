interface UserBookInteraction {
  interactionId: string;
  userId: string;
  bookId: string;
  isFavorite: boolean;
  isSaved: boolean;
  status: string | null;
  rating: number | null;
  user?: {
    userId: string;
    username: string;
    email: string;
  };
  book?: {
    bookId: string;
    title: string;
    slug: string;
  };
}

const API_BASE_URL = 'http://localhost:5094/api/v1/user-book-interactions';

async function fetchUserBookInteractions(): Promise<UserBookInteraction[]> {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(API_BASE_URL, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  if (!response.ok) {
    throw new Error('Failed to fetch interactions');
  }
  return response.json();
}

async function fetchUserBookInteractionById(id: string): Promise<UserBookInteraction> {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/${id}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  if (!response.ok) {
    throw new Error(`Failed to fetch interaction ${id}`);
  }
  return response.json();
}

async function createUserBookInteraction(interaction: Omit<UserBookInteraction, 'interactionId'>): Promise<UserBookInteraction> {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  console.log('Creating interaction:', interaction);

  const response = await fetch(API_BASE_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(interaction),
  });
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    console.error('Create interaction failed:', { 
      status: response.status, 
      statusText: response.statusText,
      errorData
    });
    throw new Error('Failed to create interaction');
  }
  return response.json();
}

async function updateUserBookInteraction(id: string, interaction: Partial<UserBookInteraction>): Promise<UserBookInteraction> {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  console.log('Updating interaction:', { id, data: interaction });

  // Only send fields that should be updated (exclude ids, user, book objects)
  const payload = {
    isFavorite: interaction.isFavorite,
    isSaved: interaction.isSaved,
    status: interaction.status,
    rating: interaction.rating
  };

  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(payload),
  });
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    console.error('Update interaction failed:', {
      status: response.status,
      statusText: response.statusText,
      errorData,
      payload
    });
    throw new Error(`Failed to update interaction ${id}: ${response.statusText}`);
  }
  return response.json();
}

async function deleteUserBookInteraction(id: string): Promise<void> {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'DELETE',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  if (!response.ok) {
    throw new Error(`Failed to delete interaction ${id}`);
  }
}

async function getUserBookInteractionByUserAndBook(userId: string, bookId: string): Promise<UserBookInteraction | null> {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    throw new Error('Authentication token not found');
  }

  const response = await fetch(`${API_BASE_URL}/user-book?userId=${userId}&bookId=${bookId}`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });

  if (response.status === 404) {
    return null;
  }

  if (!response.ok) {
    console.error(`Failed to fetch user-book interaction: ${response.status} ${response.statusText}`);
    throw new Error('Failed to fetch user-book interaction');
  }

  return response.json();
}

async function getUserBookInteraction(bookId: string): Promise<UserBookInteraction | null> {
  const token = localStorage.getItem('accessToken');
  const userId = localStorage.getItem('userId');
  
  if (!token || !userId) {
    throw new Error('Authentication token or user ID not found');
  }

  try {
    // Try the direct method first
    return await getUserBookInteractionByUserAndBook(userId, bookId);
  } catch (error) {
    console.log('Failed with direct method, trying fallback', error);
    
    // Fallback to filtering all interactions
    const response = await fetch(`${API_BASE_URL}?bookId=${bookId}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });

    if (response.status === 404) {
      return null;
    }

    if (!response.ok) {
      throw new Error('Failed to fetch user interaction');
    }

    const interactions = await response.json();
    return interactions.length > 0 ? interactions[0] : null;
  }
}

export type { UserBookInteraction };
export {
  fetchUserBookInteractions,
  fetchUserBookInteractionById,
  createUserBookInteraction,
  updateUserBookInteraction,
  deleteUserBookInteraction,
  getUserBookInteraction,
  getUserBookInteractionByUserAndBook
}; 