namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        public bool Equals(Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other)
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return EqualityComparer<TKeyToIndexMap>.Default.Equals(self._indexByKey, other._indexByKey) &&
                EqualityComparer<TIndexToValueMap>.Default.Equals(self._valueByIndex, other._valueByIndex);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other && Equals(other);

        public override int GetHashCode()
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return HashCode.Combine(
                EqualityComparer<TKeyToIndexMap>.Default.GetHashCode(self._indexByKey),
                EqualityComparer<TIndexToValueMap>.Default.GetHashCode(self._valueByIndex));
        }

        public static bool operator ==(
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) => left.Equals(right);

        public static bool operator !=(
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) => !left.Equals(right);
    }
}
