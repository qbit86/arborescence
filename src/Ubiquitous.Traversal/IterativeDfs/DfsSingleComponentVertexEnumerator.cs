namespace Ubiquitous.Traversal
{
    using System.Collections;
    using System.Collections.Generic;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsVertexStep<TVertex>>, IEnumerator<DfsVertexStep<TVertex>>
    {
        public IEnumerator<DfsVertexStep<TVertex>> GetEnumerator()
        {
            DfsSingleComponentVertexEnumerator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<DfsVertexStep<TVertex>> IEnumerable<DfsVertexStep<TVertex>>.GetEnumerator() => GetEnumerator();

        public bool MoveNext() => throw new System.NotImplementedException();

        public void Reset() => throw new System.NotImplementedException();

        public DfsVertexStep<TVertex> Current => throw new System.NotImplementedException();

        object IEnumerator.Current => Current;

        public void Dispose() => throw new System.NotImplementedException();
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
