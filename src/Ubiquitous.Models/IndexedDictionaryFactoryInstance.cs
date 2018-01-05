namespace Ubiquitous
{
    public struct IndexedDictionaryFactoryInstance<T> : IFactoryConcept<IndexedAdjacencyListGraph, IndexedDictionary<T, T[]>>
    {
        public IndexedDictionary<T, T[]> Acquire(IndexedAdjacencyListGraph context)
        {
            return IndexedDictionary.Create(new T[context.VertexCount]);
        }

        public void Release(IndexedAdjacencyListGraph context, IndexedDictionary<T, T[]> value)
        {
        }
    }
}
