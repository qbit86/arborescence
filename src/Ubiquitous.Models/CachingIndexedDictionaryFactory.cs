using System;

namespace Ubiquitous
{
    public struct CachingIndexedDictionaryFactory<T> : IFactory<IndexedAdjacencyListGraph, IndexedDictionary<T, T[]>>
    {
        private IndexedDictionary<T, T[]>? _cachedInstance;

        public IndexedDictionary<T, T[]> Acquire(IndexedAdjacencyListGraph context)
        {
            int vertexCount = context.VertexCount;
            if (!_cachedInstance.HasValue)
                return IndexedDictionary.Create(new T[vertexCount]);

            IndexedDictionary<T, T[]> result = _cachedInstance.Value;
            if (result.Count != vertexCount)
                return IndexedDictionary.Create(new T[vertexCount]);

            _cachedInstance = null;
            return result;
        }

        public void Release(IndexedAdjacencyListGraph context, IndexedDictionary<T, T[]> value)
        {
            if (value.BackingStore != null)
            {
                Array.Clear(value.BackingStore, 0, value.BackingStore.Length);
            }

            _cachedInstance = null;
        }

        public void Warmup(int vertexCount)
        {
            if (!_cachedInstance.HasValue || _cachedInstance.Value.Count != vertexCount)
                _cachedInstance = IndexedDictionary.Create(new T[vertexCount]);
        }
    }
}
