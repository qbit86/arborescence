namespace Ubiquitous.Collections
{
    using System.Diagnostics.CodeAnalysis;

    public interface IContainer<T>
    {
        bool IsEmpty { get; }
        void Add(T item);
        bool TryTake([MaybeNullWhen(false)] out T result);
    }
}
