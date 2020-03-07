namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsVertexStep<TVertex>>, IEnumerator<DfsVertexStep<TVertex>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
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

        public bool MoveNext() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        public DfsVertexStep<TVertex> Current => throw new NotImplementedException();

        object IEnumerator.Current => Current;

        public void Dispose() => throw new NotImplementedException();
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
