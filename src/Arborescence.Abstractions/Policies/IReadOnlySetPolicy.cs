namespace Arborescence
{
    /// <summary>
    /// Defines a method to check if a set contains a specific item.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the set.</typeparam>
    public interface IReadOnlySetPolicy<in TSet, in TElement>
    {
        /// <summary>
        /// Determines whether the set contains the specified item.
        /// </summary>
        /// <param name="items">The set in which to locate the item.</param>
        /// <param name="item">The item to locate in the set.</param>
        /// <returns><c>true</c> if the set contains the specified item; otherwise, <c>false</c>.</returns>
        bool Contains(TSet items, TElement item);
    }
}
