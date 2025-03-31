export interface Author {
  authorPersonId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  royaltyRate: number;
}

export interface AuthorsResponse {
  items: Author[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
} 