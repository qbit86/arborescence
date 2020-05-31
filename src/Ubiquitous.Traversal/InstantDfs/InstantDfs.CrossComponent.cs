namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    // ReSharper disable UnusedTypeParameter
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

            ColorMapPolicy.Clear(colorMap);

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
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

            ColorMapPolicy.Clear(colorMap);

            handler.OnStartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
