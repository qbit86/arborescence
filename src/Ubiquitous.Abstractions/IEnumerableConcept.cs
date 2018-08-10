namespace Ubiquitous
{
    public interface IEnumerableConcept<in TEnumerable, out TEnumerator>
    {
        TEnumerator GetEnumerator(TEnumerable collection);
    }
}
