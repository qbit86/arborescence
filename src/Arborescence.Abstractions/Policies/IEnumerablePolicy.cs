namespace Arborescence
{
    public interface IEnumerablePolicy<in TCollection, out TEnumerator>
    {
        TEnumerator GetEnumerator(TCollection collection);

        TEnumerator GetEmptyEnumerator();
    }
}
