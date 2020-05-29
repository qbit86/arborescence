namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy,
        TColorMapPolicy>
    {
        public IEnumerator<DfsStep<TEdge>> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            throw new NotImplementedException();
        }

        public IEnumerator<DfsStep<TEdge>> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            throw new NotImplementedException();
        }
    }
}
