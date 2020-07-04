namespace Arborescence
{
    /// <summary>
    /// Defines methods to support adding and taking items for the container.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the container.</typeparam>
    public interface IContainerPolicy<in TContainer, TElement>
    {
        /// <summary>
        /// Adds the item to the container.
        /// </summary>
        /// <param name="container">The container to add to.</param>
        /// <param name="item">The item to be added to the container.</param>
        void Add(TContainer container, TElement item);

        /// <summary>
        /// Attempts to remove and return the item from the container.
        /// </summary>
        /// <param name="container">The container to take from.</param>
        /// <param name="result">
        /// When this method returns, contains the removed item, if the item was removed and returned successfully.
        /// If no item was available to be removed, the value is unspecified.
        /// </param>
        /// <returns><c>true</c> if the item was removed and returned successfully; otherwise, <c>false</c>.</returns>
        bool TryTake(TContainer container, out TElement result);
    }
}
