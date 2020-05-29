namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public IEnumerator<DfsStep<TVertex>> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return EnumerateVerticesCore(graph, vertices, colorMap);
        }

        public IEnumerator<DfsStep<TVertex>> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return EnumerateVerticesCore(graph, vertices, colorMap, startVertex);
        }

        private IEnumerator<DfsStep<TVertex>> EnumerateVerticesCore<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            Debug.Assert(vertices != null, "vertices != null");

            throw new NotImplementedException();
        }

        private IEnumerator<DfsStep<TVertex>> EnumerateVerticesCore<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            Debug.Assert(vertices != null, "vertices != null");

            throw new NotImplementedException();
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
