namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    /// <summary>
    /// Provides a dictionary to use when there is a mapping from a key to an <see cref="int"/> from a contiguous range.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
    /// <typeparam name="TIndexToValueMap">The type of the mapping from an <see cref="int"/> to a value.</typeparam>
    public readonly partial struct Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> :
        IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>,
        IEquatable<Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>>
        where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
        where TIndexToValueMap : IDictionary<int, TValue>
    {
        private readonly TKeyToIndexMap _indexByKey;
        private readonly TIndexToValueMap _valueByIndex;

        internal Int32IndirectDictionary(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
        {
            _indexByKey = indexByKey;
            _valueByIndex = valueByIndex;
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            var self = this;
            if (!self._indexByKey.TryGetValue(key, out int index))
                ThrowHelper.ThrowKeyNotFoundException();
            self._valueByIndex.Add(index, value);
        }

        /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
        public int Count => (_valueByIndex?.Count).GetValueOrDefault();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey"/>
        public bool ContainsKey(TKey key)
        {
            var self = this;
            if (self._indexByKey is not { } indexByKey || self._valueByIndex is not { } valueByIndex)
                return false;
            return indexByKey.TryGetValue(key, out int index) && valueByIndex.ContainsKey(index);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found;
        /// otherwise, the value is unspecified.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// contains an element that has the specified key;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            var self = this;
            if (self._indexByKey is not { } indexByKey || self._valueByIndex is not { } valueByIndex)
                return None(out value);
            return indexByKey.TryGetValue(key, out int index)
                ? valueByIndex.TryGetValue(index, out value)
                : None(out value);
        }

        private void Put(TKey key, TValue value)
        {
            var self = this;
            if (!self._indexByKey.TryGetValue(key, out int index))
                ThrowHelper.ThrowKeyNotFoundException();
            var valueByIndex = self._valueByIndex;
            valueByIndex[index] = value;
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.this"/>
        public TValue this[TKey key]
        {
            get
            {
                if (key is null)
                    return ThrowHelper.ThrowArgumentNullException<TValue>(nameof(key));
                return TryGetValueCore(key, out var value)
                    ? value
                    : ThrowHelper.ThrowKeyNotFoundException<TValue>();
            }
            set => Put(key, value);
        }
    }
}
