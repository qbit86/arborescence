namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    public readonly partial struct Int32Dictionary<TValue, TValueList> :
        IReadOnlyDictionary<int, TValue>, IDictionary<int, TValue>,
        IEquatable<Int32Dictionary<TValue, TValueList>>
        where TValueList : IList<TValue>
    {
        private readonly TValueList _values;

        internal Int32Dictionary(TValueList values) => _values = values;

        /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
        public int Count => (_values?.Count).GetValueOrDefault();

        /// <inheritdoc/>
        public void Add(int key, TValue value)
        {
            TValueList values = _values;
            int count = values.Count;
            if (unchecked((uint)key > (uint)count))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));
            else if (key == count)
                values.Add(value);
            else
                ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
        }

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey"/>
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

        private void Put(int key, TValue value)
        {
            TValueList values = _values;
            int count = values.Count;
            if (unchecked((uint)key > (uint)count))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));
            else if (key == count)
                values.Add(value);
            else
                values[key] = value;
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.this"/>
        public TValue this[int key]
        {
            get => TryGetValueCore(key, out TValue? value)
                ? value
                : ThrowHelper.ThrowKeyNotFoundException<TValue>(key);
            set => Put(key, value);
        }
    }
}
