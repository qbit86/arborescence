namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexCollectionEnumerablePolicy :
        IEnumerablePolicy<IndexCollection, IndexCollectionEnumerator>
    {
        public IndexCollectionEnumerator GetEnumerator(IndexCollection collection)
        {
            return collection.Enumerate();
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
