namespace Arborescence
{
    public interface IReadOnlyMultimapPolicy<in TKey, in TMultimap, out TValues>
    {
        TValues EnumerateValues(TMultimap multimap, TKey key);

        int GetCount(TMultimap multimap);
    }
}
