namespace Ubiquitous.Collections
{
    public interface IContainer<T>
    {
        bool IsEmpty { get; }
        void Add(T item);
        bool TryTake(out T result);
    }
}
