namespace Arborescence
{
    using System;
#if NETSTANDARD2_1 || NETCOREAPP3_1
    using System.Diagnostics.CodeAnalysis;

#endif

    internal static class ArrayPrefixHelper
    {
#if NETSTANDARD2_1 || NETCOREAPP3_1
        [DoesNotReturn]
#endif
        internal static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }
}
