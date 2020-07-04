namespace Arborescence
{
    /// <summary>
    /// Defines methods to support adding and taking items for the container.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the container.</typeparam>
    public interface IContainerPolicy<in TContainer, TElement>
    {
        void Add(TContainer container, TElement item);
        bool TryTake(TContainer container, out TElement result);
    }
}
