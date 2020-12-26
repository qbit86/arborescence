namespace Arborescence
{
#if NETSTANDARD2_1 || NETCOREAPP3_1
    using System.Diagnostics.CodeAnalysis;

#endif

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
#if NETSTANDARD2_1 || NETCOREAPP3_1
        bool TryTake(TContainer container, [MaybeNullWhen(false)] out TElement result);
#else
        bool TryTake(TContainer container, out TElement result);
#endif
    }
}
