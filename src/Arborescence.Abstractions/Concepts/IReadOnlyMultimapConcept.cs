namespace Arborescence
{
    public interface IReadOnlyMultimapConcept<in TMultimap, in TKey, out TValues>
    {
        TValues EnumerateValues(TMultimap multimap, TKey key);

        int GetCount(TMultimap multimap);
    }
}
