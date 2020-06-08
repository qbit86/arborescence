namespace Ubiquitous
{
    public interface ISetPolicy<in TSet, in TItem>
    {
        bool Contains(TSet items, TItem item);
        void Add(TSet items, TItem item);
        void Clear(TSet items);
    }
}
