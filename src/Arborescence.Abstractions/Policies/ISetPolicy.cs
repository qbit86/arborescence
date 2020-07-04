namespace Arborescence
{
    /// <summary>
    /// Defines methods to support adding and checking items for the set.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the set.</typeparam>
    public interface ISetPolicy<in TSet, in TElement>
    {
        bool Contains(TSet items, TElement item);
        void Add(TSet items, TElement item);
    }
}
