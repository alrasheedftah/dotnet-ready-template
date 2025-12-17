namespace Domain.Shared;

public class DomainException : Exception
{
    public string Code { get; }
    public IReadOnlyDictionary<string, object?>? Args { get; }

    public DomainException(string code, string? message = null,
        IReadOnlyDictionary<string, object?>? args = null,
        Exception? inner = null)
        : base(message ?? code, inner)
    {
        Code = code;
        Args = args;
    }
}
