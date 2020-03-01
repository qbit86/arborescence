namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public void Traverse<TVertexEnumerator, THandler>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                if (!ColorMapPolicy.TryGetValue(colorMap, u, out Color color))
                    color = Color.None;

                if (color != Color.None && color != Color.White)
                    continue;

                handler.StartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }

        public void Traverse<TVertexEnumerator, THandler>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, THandler handler, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                if (!ColorMapPolicy.TryGetValue(colorMap, u, out Color color))
                    color = Color.None;

                if (color != Color.None && color != Color.White)
                    continue;

                handler.StartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }
    }
}
