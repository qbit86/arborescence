namespace Arborescence
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ArgumentOutOfRangeExceptionHelpers
    {
        [DoesNotReturn]
        internal static void Throw(string? paramName) => throw new ArgumentOutOfRangeException(paramName);

        [DoesNotReturn]
        internal static void ThrowNegative<T>(string? paramName, T value)
        {
            string message = $"{paramName} ('{value}') must be a non-negative value.";
            throw new ArgumentOutOfRangeException(paramName, value, message);
        }
    }
}
