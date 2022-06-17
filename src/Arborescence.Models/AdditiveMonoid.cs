#if NET7_0_OR_GREATER

using System.Numerics;

namespace Arborescence.Models
{
    public readonly struct AdditiveMonoid<T> : IMonoid<T>
        where T: IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        public T Identity => T.AdditiveIdentity;

        public T Combine(T left, T right) => left + right;
    }
}

#endif
