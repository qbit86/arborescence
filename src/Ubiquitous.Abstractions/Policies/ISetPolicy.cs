namespace Arborescence
{
    public interface ISetPolicy<in TSet, in TItem>
    {
        bool Contains(TSet items, TItem item);
        void Add(TSet items, TItem item);
    }
}
