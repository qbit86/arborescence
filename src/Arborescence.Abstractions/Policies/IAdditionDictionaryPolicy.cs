namespace Arborescence
{
    public interface IAdditionDictionaryPolicy<in TKey, in TValue, in TDictionary>
    {
        void Add(TDictionary dictionary, TKey key, TValue value);
    }
}
