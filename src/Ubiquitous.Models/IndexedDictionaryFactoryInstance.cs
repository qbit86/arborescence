namespace Ubiquitous
{
    public struct IndexedDictionaryFactoryInstance<T> : IFactoryConcept<IndexedAdjacencyListGraph, IndexedDictionary<T, T[]>>
    {
        public IndexedDictionary<T, T[]> Acquire(IndexedAdjacencyListGraph graph)
        {
            return IndexedDictionary.Create(new T[graph.VertexCount]);
        }

        public void Release(IndexedAdjacencyListGraph graph, IndexedDictionary<T, T[]> value)
        {
        }
    }
}
