namespace Arborescence
{
    public interface IDictionaryAddition<in TKey, in TValue, in TDictionary>
    {
        void Add(TDictionary dictionary, TKey key, TValue value);
    }
}
