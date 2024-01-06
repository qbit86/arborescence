namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EqualityComparerEquatable
    {
        public static EqualityComparerEquatable<T, TComparer> Create<T, TComparer>(T value, TComparer comparer)
            where TComparer : IEqualityComparer<T> => new(value, comparer);
    }

    public readonly struct EqualityComparerEquatable<T, TComparer> : IEquatable<T>
        where TComparer : IEqualityComparer<T>
    {
        private readonly T _value;
        private readonly TComparer _comparer;

        public EqualityComparerEquatable(T value, TComparer comparer)
        {
            _value = value;
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(T other) => _comparer.Equals(_value, other);
    }
}