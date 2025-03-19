using System.Text.RegularExpressions;

namespace Publisher.Application.Utils;

public static class SlugGenerator
{
    public static string GenerateSlug(string title)
    {
        // Remove special characters
        var slug = Regex.Replace(title.ToLower(), @"[^a-z0-9\s-]", "");
        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");
        // Remove multiple hyphens
        slug = Regex.Replace(slug, @"-+", "-");
        // Trim hyphens from start/end
        return slug.Trim('-');
    }
    
    public static string MakeSlugUnique(string baseSlug, int attempt = 1)
    {
        if (attempt == 1)
            return baseSlug;
            
        return $"{baseSlug}-{attempt}";
    }
}