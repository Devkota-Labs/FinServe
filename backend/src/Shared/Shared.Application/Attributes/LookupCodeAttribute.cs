namespace Shared.Application.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public sealed class LookupCodeAttribute(string code, string label) : Attribute
{
    public string Code { get; } = code;
    public string Label { get; } = label;
}
