namespace Arborescence
{
    /// <summary>
    /// Defines methods to support adding and checking items for the set.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the set.</typeparam>
    public interface ISetPolicy<in TSet, in TElement>
    {
        /// <summary>
        /// Determines whether the set contains the specified item.
        /// </summary>
        /// <param name="items">The set in which to locate the item.</param>
        /// <param name="item">The item to locate in the set.</param>
        /// <returns><c>true</c> if the set contains the specified item; otherwise, <c>false</c>.</returns>
        bool Contains(TSet items, TElement item);

        /// <summary>
        /// Adds the item to the set.
        /// </summary>
        /// <param name="items">The set to add to.</param>
        /// <param name="item">The item to be added to the set.</param>
        void Add(TSet items, TElement item);
    }
}
