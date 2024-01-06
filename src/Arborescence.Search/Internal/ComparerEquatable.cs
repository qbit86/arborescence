namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal readonly struct ComparerEquatable<T, TComparer> : IEquatable<T>
        where TComparer : IComparer<T>
    {
        private readonly T _value;
        private readonly TComparer _comparer;

        internal ComparerEquatable(T value, TComparer comparer)
        {
            _value = value;
            _comparer = comparer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(T? other) => _comparer.Compare(_value, other) == 0;
    }
}
