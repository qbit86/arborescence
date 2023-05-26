namespace Arborescence
{
    public interface IEnumeratorProvider<in TCollection, out TEnumerator>
    {
        TEnumerator GetEnumerator(TCollection collection);

        TEnumerator GetEmptyEnumerator();
    }
}
