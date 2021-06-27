namespace Arborescence.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(string argument) =>
            throw new ArgumentOutOfRangeException(argument);
    }
}
