namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/MemoryExtensions.cs

    internal static class MemoryExtensions
    {
        /// <summary>
        /// Creates a new span over the target array builder.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Span<T> AsSpan<T>(this ArrayBuilder<T> arrayBuilder)
        {
            return new Span<T>(arrayBuilder.Buffer, 0, arrayBuilder.Count);
        }

        /// <summary>
        /// Creates a new span over the target array prefix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Span<T> AsSpan<T>(this ArrayPrefix<T> arrayPrefix)
        {
            return new Span<T>(arrayPrefix.Array, 0, arrayPrefix.Count);
        }
    }
}
