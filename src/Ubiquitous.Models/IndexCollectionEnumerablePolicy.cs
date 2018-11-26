namespace Ubiquitous.Models
{
    public readonly struct IndexCollectionEnumerablePolicy :
        IEnumerablePolicy<IndexCollection, IndexCollectionEnumerator>
    {
        public IndexCollectionEnumerator GetEnumerator(IndexCollection collection)
        {
            return collection.GetIterator();
        }
    }
}
