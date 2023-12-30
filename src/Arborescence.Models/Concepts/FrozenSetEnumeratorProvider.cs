#if NET8_0_OR_GREATER
namespace Arborescence.Models
{
    using System.Collections.Frozen;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides an enumerator over the <see cref="FrozenSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public readonly struct FrozenSetEnumeratorProvider<T> : IEnumeratorProvider<FrozenSet<T>, FrozenSet<T>.Enumerator>
    {
        /// <inheritdoc/>
        public FrozenSet<T>.Enumerator GetEnumerator(FrozenSet<T> collection) =>
            collection?.GetEnumerator() ?? GetEmptyEnumerator();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FrozenSet<T>.Enumerator GetEmptyEnumerator() => FrozenSet<T>.Empty.GetEnumerator();
    }
}
#endif
