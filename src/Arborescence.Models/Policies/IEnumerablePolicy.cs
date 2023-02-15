namespace Arborescence
{
    internal interface IEnumerablePolicy<in TCollection, out TEnumerator>
    {
        TEnumerator GetEnumerator(TCollection collection);
        TEnumerator GetEmptyEnumerator();
    }
}
