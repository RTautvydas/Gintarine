using System.Text.RegularExpressions;

namespace Gintarine.Services.Validators;

public static class PostCodeValidator
{
    private static readonly Regex PostcodeRegex = new(@"^\d{5}$", RegexOptions.Compiled);

    public static bool IsValidPostcode(string postcode)
    {
        return !string.IsNullOrEmpty(postcode) && PostcodeRegex.IsMatch(postcode);
    }
}