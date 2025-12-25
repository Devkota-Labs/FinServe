namespace Lookup.Domain.Entities;

public sealed class LookupItem(int id, string code, string displayName)
{
    public int Id { get; } = id;
    public string Code { get; } = code;
    public string DisplayName { get; } = displayName;
}
