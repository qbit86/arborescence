namespace Ubiquitous
{
    public readonly struct IndexCollectionEnumerableConcept :
        IEnumerableConcept<IndexCollection, IndexCollection.Enumerator>
    {
        public IndexCollection.Enumerator GetEnumerator(IndexCollection collection)
        {
            return collection.GetEnumerator();
        }
    }
}
