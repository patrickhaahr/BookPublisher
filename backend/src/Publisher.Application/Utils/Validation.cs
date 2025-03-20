using System.Text.RegularExpressions;

namespace Publisher.Application.Utils;

public static class Validation
{
    public static bool IsValidGuid(Guid id)
    {
        // A Guid is valid if it's not Guid.Empty
        return id != Guid.Empty;
    }

    // Check if all entities exist in the database
    public static async Task<bool> AllEntitiesExistAsync<TId, TEntity>(
        IEnumerable<TId> ids, 
        Func<TId, CancellationToken, Task<TEntity?>> getByIdAsync, 
        CancellationToken token = default)
    {
        var tasks = ids.Select(id => getByIdAsync(id, token));
        var results = await Task.WhenAll(tasks);
        return results.All(entity => entity is not null);
    }

    public static bool IsValidBase64Image(string? base64String)
    {
        Console.WriteLine($"Validating base64: {base64String?.Substring(0, Math.Min(100, base64String.Length))}...");
        if (string.IsNullOrWhiteSpace(base64String))
            return false;

        // Remove leading or trailing whitespace
        base64String = base64String.Trim();

        // Check length (1.5 MB)
        if (base64String.Length > 1500000)
            return false;

        string base64Data = base64String;

        // Handle data URI format (e.g., "data:image/jpeg;base64,...")
        if (base64String.StartsWith("data:image"))
        {
            var parts = base64String.Split(',');
            if (parts.Length != 2)
                return false;
            // Extract and trim the base64 portion
            base64Data = parts[1].Trim();
        }

        // Ensure the string contains only valid base64 characters
        if (!Regex.IsMatch(base64Data, @"^[a-zA-Z0-9\+/]*={0,2}$"))
            return false;

        // Ensure the length is a multiple of 4 (required for valid base64)
        if (base64Data.Length % 4 != 0)
            return false;

        try
        {
            // Attempt to decode the base64 string
            Convert.FromBase64String(base64Data);
            return true;
        }
        catch (FormatException)
        {
            // Return false if decoding fails due to invalid format
            return false;
        }
    }
}