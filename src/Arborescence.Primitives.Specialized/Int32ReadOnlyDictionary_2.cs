namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using static TryHelpers;

    public readonly partial struct Int32ReadOnlyDictionary<TValue, TValueList> :
        IReadOnlyDictionary<int, TValue>,
        IEquatable<Int32ReadOnlyDictionary<TValue, TValueList>>
        where TValueList : IReadOnlyList<TValue>
    {
        private readonly TValueList _items;

        internal Int32ReadOnlyDictionary(TValueList items) => _items = items;

        public int Count
        {
            get
            {
                Int32ReadOnlyDictionary<TValue, TValueList> self = this;
                return self._items is null ? 0 : self.CountUnchecked;
            }
        }

        private int CountUnchecked => _items.Count;

        public bool ContainsKey(int key) => unchecked((uint)key < (uint)_items.Count);

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            Int32ReadOnlyDictionary<TValue, TValueList> self = this;
            if (self._items is null)
                return None(out value);
            return unchecked((uint)key >= (uint)self._items.Count)
                ? None(out value)
                : Some(self._items[key], out value);
        }

        public TValue this[int key]
        {
            get
            {
                if (TryGetValueCore(key, out TValue? value))
                    return value;
                throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");
            }
        }
    }
}
