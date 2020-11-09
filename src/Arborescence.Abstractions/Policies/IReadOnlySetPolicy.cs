namespace Arborescence
{
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
