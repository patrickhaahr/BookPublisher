namespace Publisher.Application.Utils;

public static class Validation
{
    public static bool IsValidGuid(Guid id)
    {
        // A Guid is valid if it's not Guid.Empty
        return id != Guid.Empty;
    }
}