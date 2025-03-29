import type { LoginFormValues, LoginResponse, RegisterFormValues, RegisterResponse } from '@/types';

const API_BASE_URL = 'http://localhost:5094/api/v1';

/**
 * Login user with email and password
 */
export const loginUser = async (data: LoginFormValues): Promise<LoginResponse> => {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message || 'Login failed');
  }

  return await response.json();
};

/**
 * Register a new user
 */
export const registerUser = async (
  data: Omit<RegisterFormValues, 'confirmPassword'>
): Promise<RegisterResponse> => {
  const response = await fetch(`${API_BASE_URL}/auth/register`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(errorData.message || 'Registration failed');
  }

  return await response.json();
};
