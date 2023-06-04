namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    /// <summary>
    /// Provides a read-only dictionary to use when the key is an <see cref="int"/> index in the contiguous range
    /// and the default comparer is used.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in this dictionary.</typeparam>
    /// <typeparam name="TValueList">The type of the underlying list of the values.</typeparam>
    public readonly partial struct Int32ReadOnlyDictionary<TValue, TValueList> :
        IReadOnlyDictionary<int, TValue>,
        IEquatable<Int32ReadOnlyDictionary<TValue, TValueList>>
        where TValueList : IReadOnlyList<TValue>
    {
        private readonly TValueList _values;

        internal Int32ReadOnlyDictionary(TValueList values) => _values = values;

        /// <inheritdoc/>
        public int Count => (_values?.Count).GetValueOrDefault();

        /// <inheritdoc/>
        public bool ContainsKey(int key) => unchecked((uint)key < (uint)Count);

        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found;
        /// otherwise, the value is unspecified.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Int32ReadOnlyDictionary{TValue, TValueList}"/>
        /// contains an element that has the specified key;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_values is not { } values)
                return None(out value);
            return unchecked((uint)key >= (uint)values.Count)
                ? None(out value)
                : Some(values[key], out value);
        }

        /// <inheritdoc/>
        public TValue this[int key] => TryGetValueCore(key, out TValue? value)
            ? value
            : ThrowHelper.ThrowKeyNotFoundException<TValue>(key);
    }
}
