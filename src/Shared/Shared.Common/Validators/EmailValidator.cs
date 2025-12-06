using System.Text.RegularExpressions;

namespace Shared.Common.Validators;

public static class EmailValidator
{
    private static readonly Regex _regex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    public static bool IsValid(string? email) => !string.IsNullOrWhiteSpace(email) && _regex.IsMatch(email);
}
