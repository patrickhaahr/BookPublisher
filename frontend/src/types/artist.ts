export interface Artist {
  artistPersonId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  portfolioUrl: string; 
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