namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

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
