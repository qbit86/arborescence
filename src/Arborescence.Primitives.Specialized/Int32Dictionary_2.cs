namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct Int32Dictionary<TValue, TValueList> :
        IReadOnlyDictionary<int, TValue>, IDictionary<int, TValue>,
        IEquatable<Int32Dictionary<TValue, TValueList>>
        where TValueList : IList<TValue>
    {
        private readonly TValueList _items;

        internal Int32Dictionary(TValueList items) => _items = items;

        public int Count => (_items?.Count).GetValueOrDefault();

        public void Add(KeyValuePair<int, TValue> item) => throw new NotImplementedException();

        public void Add(int key, TValue value) => throw new NotImplementedException();

        public bool ContainsKey(int key) => unchecked((uint)key < (uint)Count);

        public TValue this[int key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
