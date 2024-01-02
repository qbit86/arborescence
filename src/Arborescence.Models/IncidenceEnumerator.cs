namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating <see cref="IncidenceEnumerator{TVertex, TNeighborEnumerator}"/> objects.
    /// </summary>
    public static class IncidenceEnumerator
    {
        /// <summary>
        /// Creates a <see cref="IncidenceEnumerator{TVertex, TNeighborEnumerator}"/>.
        /// </summary>
        /// <param name="vertex">The vertex whose neighbors are to be enumerated.</param>
        /// <param name="neighborEnumerator">The enumerator for the collection of out-neighbors.</param>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
        /// <returns>The enumerator for the endpoints of the out-edges of a given vertex.</returns>
        public static IncidenceEnumerator<TVertex, TNeighborEnumerator> Create<TVertex, TNeighborEnumerator>(
            TVertex vertex, TNeighborEnumerator neighborEnumerator)
            where TNeighborEnumerator : IEnumerator<TVertex>
        {
            if (neighborEnumerator is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborEnumerator));

            return new(vertex, neighborEnumerator);
        }
    }
}
