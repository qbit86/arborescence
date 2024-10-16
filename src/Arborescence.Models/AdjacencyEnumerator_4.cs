namespace Arborescence.Models
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides an enumerator for the heads of given edges.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public struct AdjacencyEnumerator<TVertex, TEdge, TGraph, TEdgeEnumerator> :
        IEnumerable<TVertex>, IEnumerator<TVertex>
        where TGraph : IHeadIncidence<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private TGraph _graph;
        private TEdgeEnumerator _edgeEnumerator;
        private TVertex? _current;

        internal AdjacencyEnumerator(TGraph graph, TEdgeEnumerator edgeEnumerator)
        {
            _edgeEnumerator = edgeEnumerator;
            _graph = graph;
            _current = default;
        }

        /// <inheritdoc/>
        public TVertex Current => _current!;

        object? IEnumerator.Current => Current;

        /// <inheritdoc/>
        public void Dispose() => _edgeEnumerator.Dispose();

        /// <inheritdoc/>
        public bool MoveNext()
        {
            do
            {
                if (!_edgeEnumerator.MoveNext())
                    return false;
                if (!_graph.TryGetHead(_edgeEnumerator.Current, out var neighbor))
                    continue;
                _current = neighbor;
                return true;
            } while (true);
        }

        /// <inheritdoc/>
        public void Reset() => _edgeEnumerator.Reset();

        /// <summary>
        /// Returns the current enumerator instance.
        /// </summary>
        /// <returns>The current enumerator instance.</returns>
        public AdjacencyEnumerator<TVertex, TEdge, TGraph, TEdgeEnumerator> GetEnumerator() => this;

        IEnumerator<TVertex> IEnumerable<TVertex>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
