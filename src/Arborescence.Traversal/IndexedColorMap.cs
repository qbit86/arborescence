namespace Arborescence.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public readonly struct IndexedColorMap : IReadOnlyDictionary<int, Color>, IDictionary<int, Color>
    {
        public IEnumerator<KeyValuePair<int, Color>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<int, Color> item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(KeyValuePair<int, Color> item) => throw new NotImplementedException();

        public void CopyTo(KeyValuePair<int, Color>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<int, Color> item) => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(int key, Color value) => throw new NotImplementedException();

        bool IDictionary<int, Color>.ContainsKey(int key) => throw new NotImplementedException();

        public bool Remove(int key) => throw new NotImplementedException();

        bool IDictionary<int, Color>.TryGetValue(int key, out Color value) => throw new NotImplementedException();

        bool IReadOnlyDictionary<int, Color>.ContainsKey(int key) => throw new NotImplementedException();

        bool IReadOnlyDictionary<int, Color>.TryGetValue(int key, out Color value) =>
            throw new NotImplementedException();

        public Color this[int key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        IEnumerable<int> IReadOnlyDictionary<int, Color>.Keys => throw new NotImplementedException();

        ICollection<Color> IDictionary<int, Color>.Values => throw new NotImplementedException();

        ICollection<int> IDictionary<int, Color>.Keys => throw new NotImplementedException();

        IEnumerable<Color> IReadOnlyDictionary<int, Color>.Values => throw new NotImplementedException();
    }
}
