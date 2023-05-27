namespace Arborescence
{
    public interface IDictionaryAddition<in TDictionary, in TKey, in TValue>
    {
        void Add(TDictionary dictionary, TKey key, TValue value);
    }
}
