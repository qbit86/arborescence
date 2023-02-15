namespace Arborescence
{
    public interface IMultimapPolicy<in TKey, in TMultimap, out TValueEnumerator>
    {
        TValueEnumerator GetEnumerator(TMultimap multimap, TKey key);
    }
}
