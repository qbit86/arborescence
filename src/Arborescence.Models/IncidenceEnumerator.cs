namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Provides support for creating <see cref="IncidenceEnumerator{TVertex, TNeighborEnumerator}"/> objects.
    /// </summary>
    public static class IncidenceEnumeratorFactory
    {
        /// <summary>
        /// Creates a <see cref="IncidenceEnumerator{TVertex, TNeighborEnumerator}"/>.
        /// </summary>
        /// <param name="vertex">The vertex whose neighbors are to be enumerated.</param>
        /// <param name="neighborEnumerator">The enumerator for the collection of neighbor vertices.</param>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
        /// <returns></returns>
        public static IncidenceEnumerator<TVertex, TNeighborEnumerator> Create<TVertex, TNeighborEnumerator>(
            TVertex vertex, TNeighborEnumerator neighborEnumerator)
            where TNeighborEnumerator : IEnumerator<TVertex>
        {
            if (neighborEnumerator is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborEnumerator));

            return new(vertex, neighborEnumerator);
        }
    }

    /// <summary>
    /// Provides an enumerator for the out-edges of a given vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public struct IncidenceEnumerator<TVertex, TNeighborEnumerator> :
        IEnumerable<Endpoints<TVertex>>, IEnumerator<Endpoints<TVertex>>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        private TNeighborEnumerator _neighborEnumerator;
        private readonly TVertex _vertex;

        internal IncidenceEnumerator(TVertex vertex, TNeighborEnumerator neighborEnumerator)
        {
            _vertex = vertex;
            _neighborEnumerator = neighborEnumerator;
        }

        /// <inheritdoc/>
        public Endpoints<TVertex> Current => new(_vertex, _neighborEnumerator.Current);

        object IEnumerator.Current => Endpoints.Create(_vertex, _neighborEnumerator.Current);

        /// <inheritdoc/>
        public void Dispose() => _neighborEnumerator.Dispose();

        /// <inheritdoc/>
        public bool MoveNext() => _neighborEnumerator.MoveNext();

        /// <inheritdoc/>
        public void Reset() => _neighborEnumerator.Reset();

        /// <summary>
        /// Returns the current enumerator instance.
        /// </summary>
        /// <returns>The current enumerator instance.</returns>
        public IncidenceEnumerator<TVertex, TNeighborEnumerator> GetEnumerator() => this;

        IEnumerator<Endpoints<TVertex>> IEnumerable<Endpoints<TVertex>>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
