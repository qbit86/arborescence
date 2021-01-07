namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        public void Traverse<TVertexEnumerator, TColorMap, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorMap, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            while (sources.MoveNext())
            {
                TVertex u = sources.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }

        public void Traverse<TVertexEnumerator, TColorMap, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorMap, THandler handler, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);

            while (sources.MoveNext())
            {
                TVertex u = sources.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }
    }
}
