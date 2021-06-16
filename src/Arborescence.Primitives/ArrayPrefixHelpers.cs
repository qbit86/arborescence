namespace Arborescence
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ArrayPrefixHelpers
    {
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }
}
