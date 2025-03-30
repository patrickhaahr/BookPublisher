export interface Author {
  authorId?: string;
  authorPersonId?: string;
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
}

export interface Artist {
  artistPersonId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  portfolioUrl: string;
}

export interface Cover {
  coverId: string;
  imgBase64: string;
  createdDate?: string;
  artists: Artist[];
}

export interface Book {
  bookId: string;
  title: string;
  publishDate: string;
  basePrice: number;
  slug: string;
  mediums: { mediumId: string; name: string }[];
  genres: { genreId: string; name: string }[];
  covers: { coverId: string; imgBase64: string }[];
  authors: { authorId: string; firstName: string; lastName: string }[];
}

export interface BookDetails {
  bookId: string;
  title: string;
  publishDate: string;
  basePrice: number;
  slug: string;
  covers: Cover[];
  authors: Array<{ authorPersonId: string; firstName: string; lastName: string }>;
  mediums: string[];
  genres: string[];
}

export interface BooksResponse {
  items: Book[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface BookCardProps {
  id: string;
  title: string;
  coverUrl: string;
  author: string;
}

export interface CreateBookFormValues {
  title: string;
  publishDate?: Date;
  basePrice: number;
  authorIds: string; 
  mediums: string[];
  genres: string[];
  coverImage?: File;
  coverArtistIds: string;
}

export interface EditBookFormValues {
  title?: string | null;
  publishDate?: string | null;
  basePrice?: number | null;
  authorIds?: string | null;
  mediums?: string[] | null;
  genres?: string[] | null;
  coverImage?: File | null;
  coverArtistIds?: string | null;
}

export interface CreateBookApiPayload {
  title: string;
  publishDate?: string; 
  basePrice: number;
  authorIds: string[];
  mediums: string[];
  genres: string[];
  covers: { imgBase64: string; artistIds: string[] }[];
}

export interface CreateBookApiResponse {
  bookId: string;
  title: string;
  slug: string;
}
