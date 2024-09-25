namespace Arborescence.Models
{
    using System.Runtime.CompilerServices;
#if NET5_0_OR_GREATER
    using System;
#endif

    internal static class ArrayHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T[] AllocateUninitializedArray<T>(int length)
        {
#if NET5_0_OR_GREATER
            return GC.AllocateUninitializedArray<T>(length);
#else
            return new T[length];
#endif
        }
    }
}
