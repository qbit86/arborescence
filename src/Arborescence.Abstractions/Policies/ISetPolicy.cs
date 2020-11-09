namespace Arborescence
{
    /// <summary>
    /// Defines methods to support adding and checking items for a set.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the set.</typeparam>
    public interface ISetPolicy<in TSet, in TElement> : IReadOnlySetPolicy<TSet, TElement>
    {
        /// <summary>
        /// Adds the item to the set.
        /// </summary>
        /// <param name="items">The set to add to.</param>
        /// <param name="item">The item to be added to the set.</param>
        void Add(TSet items, TElement item);
    }
}
