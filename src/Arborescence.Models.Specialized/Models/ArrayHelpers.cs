namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

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
