namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    public readonly partial struct Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> :
        IReadOnlyDictionary<TKey, TValue>,
        IEquatable<Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>>
        where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
        where TIndexToValueMap : IReadOnlyDictionary<int, TValue>
    {
        private readonly TKeyToIndexMap _indexByKey;
        private readonly TIndexToValueMap _valueByIndex;

        internal Int32IndirectReadOnlyDictionary(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
        {
            _indexByKey = indexByKey;
            _valueByIndex = valueByIndex;
        }

        public int Count => (_valueByIndex?.Count).GetValueOrDefault();

        public bool ContainsKey(TKey key)
        {
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            if (self._indexByKey is not { } indexByKey || self._valueByIndex is not { } valueByIndex)
                return false;
            return indexByKey.TryGetValue(key, out int index) && valueByIndex.ContainsKey(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            if (self._indexByKey is not { } indexByKey || self._valueByIndex is not { } valueByIndex)
                return None(out value);
            return indexByKey.TryGetValue(key, out int index)
                ? valueByIndex.TryGetValue(index, out value)
                : None(out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key is null)
                    return ThrowHelper.ThrowArgumentNullException<TValue>(nameof(key));
                return TryGetValueCore(key, out TValue? value)
                    ? value
                    : ThrowHelper.ThrowKeyNotFoundException<TValue>();
            }
        }
    }
}
