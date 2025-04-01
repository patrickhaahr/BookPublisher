using Microsoft.EntityFrameworkCore;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;
using System.Linq;

namespace Publisher.Infrastructure.Repositories;

internal static class BookRepositoryCompiledQueries
{
    // A compiled query for books with all necessary includes
    public static readonly Func<AppDbContext, IQueryable<Book>> GetBooksWithIncludes = 
        context => context.Books
            .Include(b => b.Covers)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .Include(b => b.BookMediums)
                .ThenInclude(bm => bm.Medium);

    // Helper methods to apply filters - these are not compiled queries themselves
    // but will be used by the repository to build filtered queries
    
    // Count function with filter parameters using async
    public static readonly Func<AppDbContext, string?, string?, string?, string?, string?, Task<int>> CountBooksAsync =
        async (context, title, author, genre, medium, year) =>
        {
            var query = context.Books.AsQueryable();
            
            // Apply title filter
            if (!string.IsNullOrEmpty(title))
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{title}%"));
                
            // Apply author filter
            if (!string.IsNullOrEmpty(author))
                query = query.Where(b => b.BookPersons.Any(bp =>
                    EF.Functions.Like(bp.Author.FirstName, $"%{author}%") ||
                    EF.Functions.Like(bp.Author.LastName, $"%{author}%") ||
                    EF.Functions.Like(bp.Author.FirstName + " " + bp.Author.LastName, $"%{author}%")));
                    
            // Apply genre filter
            if (!string.IsNullOrEmpty(genre) && Enum.TryParse<GenreEnum>(genre, true, out var genreEnum))
                query = query.Where(b => b.BookGenres.Any(bg => bg.GenreId == (int)genreEnum));
                
            // Apply medium filter
            if (!string.IsNullOrEmpty(medium) && Enum.TryParse<MediumEnum>(medium, true, out var mediumEnum))
                query = query.Where(b => b.BookMediums.Any(bm => bm.MediumId == (int)mediumEnum));
                
            // Apply year filter
            if (!string.IsNullOrEmpty(year))
                query = query.Where(b => EF.Functions.Like(b.PublishDate.Year.ToString(), $"{year}%"));
                
            return await query.CountAsync();
        };

    // Simple paging that can be applied to any queryable
    public static readonly Func<IQueryable<Book>, int, int, IQueryable<Book>> ApplyPaging =
        (query, skip, take) => query.Skip(skip).Take(take);
} 