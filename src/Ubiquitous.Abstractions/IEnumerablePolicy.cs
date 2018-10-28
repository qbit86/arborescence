namespace Ubiquitous
{
    public interface IEnumerablePolicy<in TEnumerable, out TEnumerator>
    {
        TEnumerator GetEnumerator(TEnumerable collection);
    }
}
