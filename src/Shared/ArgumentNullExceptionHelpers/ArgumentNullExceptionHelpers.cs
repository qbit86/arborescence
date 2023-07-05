namespace Arborescence
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ArgumentNullExceptionHelpers
    {
        [DoesNotReturn]
        internal static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
    }
}
