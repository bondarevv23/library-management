namespace LibraryManagementSystem.Utilities;

public static class ValidationUtilities
{
    public static bool BeValidDateOnly(string? dateString) => DateOnly.TryParse(dateString, out _);

    public static bool BePositiveValidLong(string? value) => long.TryParse(value, out long _);

    public static bool BePositiveValidInt(string? value) => int.TryParse(value, out int _);

    public static bool BePositiveMultipleOf10Int(string? value) => int.TryParse(value, out int result) && result % 10 == 0;
}
