#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        public static void ThrowKeyNotFoundException() => throw new KeyNotFoundException();

        [DoesNotReturn]
        public static void ThrowNotSupportedException() => throw new NotSupportedException();
    }
}
#endif
