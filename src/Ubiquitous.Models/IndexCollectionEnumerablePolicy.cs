namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexCollectionEnumerablePolicy :
        IEnumerablePolicy<IndexCollection, IndexCollectionEnumerator>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public IndexCollectionEnumerator GetEnumerator(IndexCollection collection)
        {
            return collection.GetValueEnumerator();
        }
    }
}
