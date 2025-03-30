export interface User {
  userId: string;
  username: string;
  email: string;
  role: string;
}

export interface UserProfile {
  userId: string;
  username: string;
  email: string;
  role: string;
}

export interface UsersResponse {
  items: User[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}