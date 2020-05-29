namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy,
        TColorMapPolicy>
    {
        public IEnumerator<DfsStep<TEdge>> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return EnumerateEdgesCore(graph, vertices, colorMap);
        }

        public IEnumerator<DfsStep<TEdge>> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return EnumerateEdgesCore(graph, vertices, colorMap, startVertex);
        }

        private IEnumerator<DfsStep<TEdge>> EnumerateEdgesCore<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            Debug.Assert(vertices != null, "vertices != null");

            ColorMapPolicy.Clear(colorMap);

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                var iterator = new DfsEdgeIterator<
                    TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                    GraphPolicy, ColorMapPolicy, graph, u, colorMap);
                while (iterator.MoveNext())
                    yield return iterator.Current;
            }
        }

        private IEnumerator<DfsStep<TEdge>> EnumerateEdgesCore<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            Debug.Assert(vertices != null, "vertices != null");

            ColorMapPolicy.Clear(colorMap);

            var startIterator = new DfsEdgeIterator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                GraphPolicy, ColorMapPolicy, graph, startVertex, colorMap);
            while (startIterator.MoveNext())
                yield return startIterator.Current;

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                var iterator = new DfsEdgeIterator<
                    TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                    GraphPolicy, ColorMapPolicy, graph, u, colorMap);
                while (iterator.MoveNext())
                    yield return iterator.Current;
            }
        }
    }
}
