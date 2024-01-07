#if NET7_0_OR_GREATER
namespace Arborescence.Search
{
    using System.Numerics;

    /// <summary>
    /// Represents a monoid for the elements of generic type <typeparamref name="T"/> with addition as the binary operation.
    /// </summary>
    internal readonly struct AdditiveMonoid<T> : IMonoid<T>
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        /// <inheritdoc/>
        public T Identity => T.AdditiveIdentity;

        /// <inheritdoc/>
        public T Combine(T left, T right) => left + right;
    }
}
#endif
