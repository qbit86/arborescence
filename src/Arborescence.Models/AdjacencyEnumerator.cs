namespace Arborescence.Models
{
    using System.Collections;
    using System.Collections.Generic;

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

        public TVertex Current => _current!;

        object? IEnumerator.Current => Current;

        public void Dispose() => _edgeEnumerator.Dispose();

        public bool MoveNext()
        {
            do
            {
                if (!_edgeEnumerator.MoveNext())
                    return false;
                if (!_graph.TryGetHead(_edgeEnumerator.Current, out TVertex? neighbor))
                    continue;
                _current = neighbor;
                return true;
            } while (true);
        }

        public void Reset() => _edgeEnumerator.Reset();

        public AdjacencyEnumerator<TVertex, TEdge, TGraph, TEdgeEnumerator> GetEnumerator() => this;

        IEnumerator<TVertex> IEnumerable<TVertex>.GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
