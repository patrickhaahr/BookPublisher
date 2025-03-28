import { useQuery } from '@tanstack/react-query';
import { createFileRoute } from '@tanstack/react-router';
import { BookCard, BookCardSkeleton } from '@/components/book-card';
import { useState, useRef, useEffect } from 'react';
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
  PaginationEllipsis,
} from '@/components/ui/pagination';
import { Input } from '@/components/ui/input';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Label } from '@/components/ui/label';
import { useDebounce } from 'use-debounce';

export const Route = createFileRoute('/books/')({
  component: Books,
});

interface Book {
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

interface BooksResponse {
  items: Book[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

const genres = [
  'Fiction', 'NonFiction', 'Fantasy', 'ScienceFiction', 'Mystery', 'Romance',
  'Thriller', 'Horror', 'HistoricalFiction', 'Adventure', 'YoungAdult',
  'Childrens', 'Biography', 'SelfHelp', 'Poetry', 'Science', 'Travel',
  'Humor', 'Programming', 'Finance', 'Epic', 'DarkFantasy',
];

const mediums = [
  'Print', 'Magazine', 'EBook', 'AudioBook', 'Novel', 'LightNovel',
  'WebNovel', 'GraphicNovel', 'Comic', 'Manga', 'Manhwa', 'Manhua',
];

function Books() {
  const [page, setPage] = useState(1);
  const [titleFilter, setTitleFilter] = useState('');
  const [debouncedTitleFilter] = useDebounce(titleFilter, 300); // Debounce by 300ms
  const [authorFilter, setAuthorFilter] = useState('');
  const [debouncedAuthorFilter] = useDebounce(authorFilter, 300); // Debounce by 300ms
  const [genreFilter, setGenreFilter] = useState<string | undefined>(undefined);
  const [mediumFilter, setMediumFilter] = useState<string | undefined>(undefined);
  const [yearFilter, setYearFilter] = useState<string | undefined>(undefined);
  const [debouncedYearFilter] = useDebounce(yearFilter, 300); // Debounce by 300ms
  const pageSize = 12;

  const titleInputRef = useRef<HTMLInputElement>(null);
  const yearInputRef = useRef<HTMLInputElement>(null);
  const authorInputRef = useRef<HTMLInputElement>(null);

  const { data: booksData, isPending, error } = useQuery({
    queryKey: ['books', page, debouncedTitleFilter, debouncedAuthorFilter, genreFilter, mediumFilter, debouncedYearFilter],
    queryFn: () => getBooks(page, pageSize, debouncedTitleFilter, debouncedAuthorFilter, genreFilter, mediumFilter, debouncedYearFilter),
    staleTime: 1000 * 60 * 5,
    gcTime: 1000 * 60 * 5,
  });

  // Restore focus to the title input after re-render if it was focused
  useEffect(() => {
    if (document.activeElement === titleInputRef.current) {
      titleInputRef.current?.focus();
    }
    if (document.activeElement === authorInputRef.current) {
      authorInputRef.current?.focus();
    }
    if (document.activeElement === yearInputRef.current) {
      yearInputRef.current?.focus();
    }
  }, [titleFilter, authorFilter, yearFilter]);

  const booksWithCovers = booksData?.items.map((book: Book) => ({
    id: book.slug,
    title: book.title,
    coverUrl: book.covers[0] ? `data:image/png;base64,${book.covers[0].imgBase64}` : '',
    author: book.authors[0] ? `${book.authors[0].firstName} ${book.authors[0].lastName}` : '',
  })) || [];

  const handlePageChange = (newPage: number) => {
    if (newPage >= 1 && newPage <= (booksData?.totalPages || 1)) {
      setPage(newPage);
    }
  };

  return (
    <div className="space-y-8">
      {/* Filter Controls - Always Visible */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:gap-6">
        <div className="flex-1">
          <Label htmlFor="title-filter" className="block mb-2 text-sm font-medium">
            Search by Title
          </Label>
          <Input
            id="title-filter"
            ref={titleInputRef}
            value={titleFilter}
            onChange={(e) => {
              setTitleFilter(e.target.value);
              setPage(1);
            }}
            placeholder="Enter book title..."
            className="w-full"
          />
        </div>

        <div className="flex-1">
          <Label htmlFor="author-filter" className="block mb-2 text-sm font-medium">
            Search by Author
          </Label>
          <Input
            id="author-filter"
            ref={authorInputRef}
            value={authorFilter}
            onChange={(e) => {
              setAuthorFilter(e.target.value);
              setPage(1);
            }}
            placeholder="Enter author name..."
            className="w-full"
          />
        </div>

        <div className="w-full sm:w-48">
          <Label htmlFor="genre-filter" className="block mb-2 text-sm font-medium">
            Filter by Genre
          </Label>
          <Select
            value={genreFilter}
            onValueChange={(value) => {
              setGenreFilter(value === 'all' ? undefined : value);
              setPage(1);
            }}
          >
            <SelectTrigger id="genre-filter">
              <SelectValue placeholder="Select genre" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All Genres</SelectItem>
              {genres.map((genre) => (
                <SelectItem key={genre} value={genre}>
                  {genre}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="w-full sm:w-48">
          <Label htmlFor="medium-filter" className="block mb-2 text-sm font-medium">
            Filter by Medium
          </Label>
          <Select
            value={mediumFilter}
            onValueChange={(value) => {
              setMediumFilter(value === 'all' ? undefined : value);
              setPage(1);
            }}
          >
            <SelectTrigger id="medium-filter">
              <SelectValue placeholder="Select medium" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All Mediums</SelectItem>
              {mediums.map((medium) => (
                <SelectItem key={medium} value={medium}>
                  {medium}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="w-full sm:w-32">
          <Label htmlFor="year-filter" className="block mb-2 text-sm font-medium">
            Filter by Year
          </Label>
          <Input
            id="year-filter"
            ref={yearInputRef}
            type="number"
            value={yearFilter || ''}
            onChange={(e) => {
              const value = e.target.value;
              setYearFilter(value);
              setPage(1);
            }}
            placeholder="YYYY"
            className="w-full"
          />
        </div>
      </div>

      {/* Results Section */}
      {error && (
        <div className="text-center text-lg font-semibold text-red-500">
          Error: {error.message}
        </div>
      )}

      <div className="space-y-8">
        {isPending ? (
          <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
            {Array.from({ length: 6 }).map((_, index) => (
              <BookCardSkeleton key={index} />
            ))}
          </div>
        ) : booksWithCovers.length === 0 ? (
          <div className="text-center text-lg font-semibold text-gray-500">
            No books found matching your filters.
          </div>
        ) : (
          <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
            {booksWithCovers.map((book) => (
              <BookCard key={book.id} {...book} />
            ))}
          </div>
        )}

        {(booksData?.totalPages ?? 0) > 1 && (
          <Pagination>
            <PaginationContent>
              <PaginationItem>
                <PaginationPrevious
                  href="#"
                  onClick={(e) => {
                    e.preventDefault();
                    handlePageChange(page - 1);
                  }}
                  className={!booksData?.hasPreviousPage ? 'pointer-events-none opacity-50' : ''}
                />
              </PaginationItem>

              <PaginationItem>
                <PaginationLink
                  href="#"
                  onClick={(e) => {
                    e.preventDefault();
                    handlePageChange(1);
                  }}
                  isActive={page === 1}
                >
                  1
                </PaginationLink>
              </PaginationItem>

              {page > 3 && (
                <PaginationItem>
                  <PaginationEllipsis />
                </PaginationItem>
              )}

              {Array.from({ length: 3 }, (_, i) => {
                const pageNum = page - 1 + i;
                return pageNum > 1 && pageNum < (booksData?.totalPages ?? 0) ? (
                  <PaginationItem key={pageNum}>
                    <PaginationLink
                      href="#"
                      onClick={(e) => {
                        e.preventDefault();
                        handlePageChange(pageNum);
                      }}
                      isActive={page === pageNum}
                    >
                      {pageNum}
                    </PaginationLink>
                  </PaginationItem>
                ) : null;
              })}

              {page < (booksData?.totalPages ?? 0) - 2 && (
                <PaginationItem>
                  <PaginationEllipsis />
                </PaginationItem>
              )}

              {(booksData?.totalPages ?? 0) > 1 && (
                <PaginationItem>
                  <PaginationLink
                    href="#"
                    onClick={(e) => {
                      e.preventDefault();
                      handlePageChange(booksData?.totalPages ?? 1);
                    }}
                    isActive={page === booksData?.totalPages}
                  >
                    {booksData?.totalPages}
                  </PaginationLink>
                </PaginationItem>
              )}

              <PaginationItem>
                <PaginationNext
                  href="#"
                  onClick={(e) => {
                    e.preventDefault();
                    handlePageChange(page + 1);
                  }}
                  className={!booksData?.hasNextPage ? 'pointer-events-none opacity-50' : ''}
                />
              </PaginationItem>
            </PaginationContent>
          </Pagination>
        )}

        {booksData && (
          <div className="text-center text-sm text-muted-foreground">
            Showing {(page - 1) * pageSize + 1} to {Math.min(page * pageSize, booksData.total)} of{' '}
            {booksData.total} books
          </div>
        )}
      </div>
    </div>
  );
}

const getBooks = async (
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
  const response = await fetch(`http://localhost:5094/api/v1/books?${queryParams}`);
  if (!response.ok) throw new Error('Failed to fetch books');
  return await response.json() as BooksResponse;
};