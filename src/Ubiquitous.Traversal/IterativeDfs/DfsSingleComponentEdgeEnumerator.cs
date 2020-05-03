namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsStep<TVertex>>, IEnumerator<DfsStep<TVertex>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        internal DfsSingleComponentEdgeEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");
        }

        public IEnumerator<DfsStep<TVertex>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose() => throw new NotImplementedException();

        public bool MoveNext() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        public DfsStep<TVertex> Current => throw new NotImplementedException();

        object IEnumerator.Current => Current;
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
