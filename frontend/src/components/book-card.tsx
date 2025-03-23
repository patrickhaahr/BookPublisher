import { Link } from '@tanstack/react-router'
import { Skeleton } from './ui/skeleton'

interface BookCardProps {
  id: string
  title: string
  coverUrl: string
  author: string
}

export function BookCardSkeleton() {
  return (
    <div className="group overflow-hidden rounded-lg border bg-card">
      <div className="aspect-[3/4] overflow-hidden bg-muted">
        <Skeleton className="h-full w-full" />
      </div>
      <div className="p-4">
        <Skeleton className="h-5 w-full" />
        <Skeleton className="mt-2 h-4 w-3/4" />
      </div>
    </div>
  )
}

export function BookCard({ id, title, coverUrl, author }: BookCardProps) {
  return (
    <Link
      to="/books/$bookId"
      params={{ bookId: id }}
      className="group overflow-hidden rounded-lg border bg-card transition-colors hover:border-primary"
    >
      <div className="aspect-[3/4] overflow-hidden bg-muted">
        <img
          src={coverUrl}
          alt={`Cover of ${title}`}
          className="h-full w-full object-cover transition-transform group-hover:scale-105"
          loading="lazy"
        />
      </div>
      <div className="p-4">
        <h3 className="font-semibold leading-none tracking-tight">{title}</h3>
        <p className="mt-2 text-sm text-muted-foreground">{author}</p>
      </div>
    </Link>
  )
} 