export interface Artist {
  personId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  portfolioUrl: string;
  coverPersons: any[]; // Could be defined more precisely if needed
}

export interface ArtistsResponse {
  items: Artist[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
} 