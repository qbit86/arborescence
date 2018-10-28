namespace Ubiquitous
{
    public readonly struct IndexCollectionEnumerablePolicy :
        IEnumerablePolicy<IndexCollection, IndexCollection.Enumerator>
    {
        public IndexCollection.Enumerator GetEnumerator(IndexCollection collection)
        {
            return collection.GetEnumerator();
        }
    }
}
