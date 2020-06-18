namespace Ubiquitous.Collections
{
    using System.Diagnostics.CodeAnalysis;

    public interface IContainer<T>
    {
        void Add(T item);
        bool TryTake([MaybeNullWhen(false)] out T result);
    }
}
