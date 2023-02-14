namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using Models;

    public static class IncidenceEnumerator
    {
        public static IncidenceEnumerator<TVertex, TNeighborEnumerator> Create<TVertex, TNeighborEnumerator>(
            TVertex vertex, TNeighborEnumerator neighborEnumerator)
            where TNeighborEnumerator : IEnumerator<TVertex>
        {
            if (neighborEnumerator is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborEnumerator));

            return new(vertex, neighborEnumerator);
        }
    }

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

        public Endpoints<TVertex> Current => new(_vertex, _neighborEnumerator.Current);

        object IEnumerator.Current => Endpoints.Create(_vertex, _neighborEnumerator.Current);

        public void Dispose() => _neighborEnumerator.Dispose();

        public bool MoveNext() => _neighborEnumerator.MoveNext();

        public void Reset() => _neighborEnumerator.Reset();

        public IncidenceEnumerator<TVertex, TNeighborEnumerator> GetEnumerator() => this;

        IEnumerator<Endpoints<TVertex>> IEnumerable<Endpoints<TVertex>>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
