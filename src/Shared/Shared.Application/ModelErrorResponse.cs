namespace Shared.Application;

public sealed record ModelErrorResponse(string Field, IEnumerable<string> Errors);
