export const MEDIUMS = [
  { id: 'Print', label: 'Print' },
  { id: 'Magazine', label: 'Magazine' },
  { id: 'EBook', label: 'E-Book' },
  { id: 'AudioBook', label: 'Audio Book' },
  { id: 'Novel', label: 'Novel' },
  { id: 'LightNovel', label: 'Light Novel' },
  { id: 'WebNovel', label: 'Web Novel' },
  { id: 'GraphicNovel', label: 'Graphic Novel' },
  { id: 'Comic', label: 'Comic' },
  { id: 'Manga', label: 'Manga' },
  { id: 'Manhwa', label: 'Manhwa' },
  { id: 'Manhua', label: 'Manhua' },
] as const;

export const MEDIUM_VALUES = MEDIUMS.map(medium => medium.id);
export type Medium = typeof MEDIUM_VALUES[number];

export const GENRES = [
  { id: 'Fiction', label: 'Fiction' },
  { id: 'NonFiction', label: 'Non-Fiction' },
  { id: 'Fantasy', label: 'Fantasy' },
  { id: 'ScienceFiction', label: 'Science Fiction' },
  { id: 'Mystery', label: 'Mystery' },
  { id: 'Romance', label: 'Romance' },
  { id: 'Thriller', label: 'Thriller' },
  { id: 'Horror', label: 'Horror' },
  { id: 'HistoricalFiction', label: 'Historical Fiction' },
  { id: 'Adventure', label: 'Adventure' },
  { id: 'YoungAdult', label: 'Young Adult' },
  { id: 'Childrens', label: 'Children\'s' },
  { id: 'Biography', label: 'Biography' },
  { id: 'SelfHelp', label: 'Self Help' },
  { id: 'Poetry', label: 'Poetry' },
  { id: 'Science', label: 'Science' },
  { id: 'Travel', label: 'Travel' },
  { id: 'Humor', label: 'Humor' },
  { id: 'Programming', label: 'Programming' },
  { id: 'Finance', label: 'Finance' },
  { id: 'Epic', label: 'Epic' },
  { id: 'DarkFantasy', label: 'Dark Fantasy' },
] as const;

export const GENRE_VALUES = GENRES.map(genre => genre.id);
export type Genre = typeof GENRE_VALUES[number];
