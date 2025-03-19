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

    public static bool IsValidBase64Image(string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String))
            return false;

        try
        {
            // Check if the string is valid base64
            Convert.FromBase64String(base64String);

            // Optionally check the content type (e.g., starts with "data:image")
            if (base64String.StartsWith("data:image"))
            {
                var parts = base64String.Split(',');
                if (parts.Length != 2)
                    return false;
                base64String = parts[1];
                Convert.FromBase64String(base64String);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}