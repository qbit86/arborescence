namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public void Traverse<TVertexEnumerator, THandler>(
            TGraph graph, TVertexEnumerator vertices, TVertex startVertex, TColorMap colorMap, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);

            throw new NotImplementedException();
        }
    }
}
