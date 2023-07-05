namespace Arborescence
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ArgumentOutOfRangeExceptionHelpers
    {
        [DoesNotReturn]
        internal static void Throw(string? paramName) => throw new ArgumentOutOfRangeException(paramName);
    }
}
