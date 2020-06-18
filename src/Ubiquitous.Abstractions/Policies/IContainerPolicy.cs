namespace Ubiquitous
{
    public interface IContainerPolicy<in TContainer, TItem>
    {
        void Add(TContainer container, TItem item);
        bool TryTake(TContainer container, out TItem result);
    }
}
