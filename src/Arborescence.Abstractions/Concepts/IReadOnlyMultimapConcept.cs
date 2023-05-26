namespace Arborescence
{
    public interface IReadOnlyMultimapConcept<in TKey, in TMultimap, out TValues>
    {
        TValues EnumerateValues(TMultimap multimap, TKey key);

        int GetCount(TMultimap multimap);
    }
}
