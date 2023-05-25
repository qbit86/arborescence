namespace Arborescence.Models
{
    using System.Runtime.CompilerServices;

    internal static class ArrayHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T[] AllocateUninitializedArray<T>(int length)
        {
#if NET5_0_OR_GREATER
            return System.GC.AllocateUninitializedArray<T>(length);
#else
            return new T[length];
#endif
        }
    }
}
