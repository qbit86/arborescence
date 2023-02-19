namespace Arborescence
{
    using System;
    using System.Collections.Generic;

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

        public bool ContainsKey(int key) => throw new NotImplementedException();

        public bool TryGetValue(int key, out TValue value) => throw new NotImplementedException();

        public TValue this[int key] => throw new NotImplementedException();
    }
}
