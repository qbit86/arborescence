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
        private readonly TValueList _items;

        internal Int32Dictionary(TValueList items) => _items = items;

        public int Count => (_items?.Count).GetValueOrDefault();

        public void Add(int key, TValue value)
        {
            TValueList items = _items;
            int count = items.Count;
            if (unchecked((uint)key > (uint)count))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));
            else if (key == count)
                items.Add(value);
            else
                ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
        }

        public bool ContainsKey(int key) => unchecked((uint)key < (uint)Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_items is not { } items)
                return None(out value);
            return unchecked((uint)key >= (uint)items.Count)
                ? None(out value)
                : Some(items[key], out value);
        }

        private void Put(int key, TValue value)
        {
            TValueList items = _items;
            int count = items.Count;
            if (unchecked((uint)key > (uint)count))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));
            else if (key == count)
                items.Add(value);
            else
                items[key] = value;
        }

        public TValue this[int key]
        {
            get => TryGetValueCore(key, out TValue? value)
                ? value
                : ThrowHelper.ThrowKeyNotFoundException<TValue>(key);
            set => Put(key, value);
        }
    }
}
