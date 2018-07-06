namespace Ubiquitous
{
    public readonly struct IndexedDictionaryFactory<T> : IMapConcept<IndexedDictionary<T, T[]>, int, T>,
        IFactory<IndexedAdjacencyListGraph, IndexedDictionary<T, T[]>>
    {
        public bool TryGet(IndexedDictionary<T, T[]> map, int key, out T value)
        {
            return map.TryGetValue(key, out value);
        }

        public bool TryPut(IndexedDictionary<T, T[]> map, int key, T value)
        {
            if ((uint)key >= (uint)map.Count)
                return false;

            map[key] = value;
            return true;
        }

        public IndexedDictionary<T, T[]> Acquire(IndexedAdjacencyListGraph context)
        {
            return IndexedDictionary.Create(new T[context.VertexCount]);
        }

        public void Release(IndexedAdjacencyListGraph context, IndexedDictionary<T, T[]> value)
        {
        }
    }
}
