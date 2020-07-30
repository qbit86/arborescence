namespace Arborescence
{
    using System;

#if NETSTANDARD2_1
    using System.Diagnostics.CodeAnalysis;
#endif

    internal static class ArrayPrefixHelper
    {
#if NETSTANDARD2_1
        [DoesNotReturn]
#endif
        internal static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }
    }
}
