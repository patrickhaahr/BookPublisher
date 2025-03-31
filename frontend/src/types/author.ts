export interface Author {
  personId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  royaltyRate: number;
  bookPersons: any[]; // Could be defined more precisely if needed
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