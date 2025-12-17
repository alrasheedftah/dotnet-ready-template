namespace Domain.Shared;

public static class Guard
{
    public static void AgainstNull(object? value, string name)
    {
        if (value is null)
            throw new DomainException(
                code: "Validation.Required",
                args: new Dictionary<string, object?> { ["field"] = name });
    }

    public static void AgainstEmpty(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(
                code: "Validation.Required",
                args: new Dictionary<string, object?> { ["field"] = name });
    }

    public static T AgainstNull<T>(T? value, string name)
    {
        if (value is null)
            throw new DomainException("Validation.Required",
                args: new Dictionary<string, object?> { ["field"] = name });

        return value;
    }    
}
