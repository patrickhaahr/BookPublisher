import { Search } from 'lucide-react'

export function SearchBar() {
  return (
    <div className="relative w-full max-w-sm">
      <Search className="absolute right-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
      <input
        type="search"
        placeholder="Search by title, author, or keywords..."
        className="flex h-9 w-full rounded-md border border-input bg-background px-3 py-1 pl-4 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50"
      />
    </div>
  )
} 