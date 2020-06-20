namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct LegacyDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
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

            while (vertices.MoveNext())
            {
                TVertex u = vertices.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                yield return new DfsStep<TVertex>(DfsStepKind.StartVertex, u);
                var iterator = new DfsVertexIterator<
                    TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                    GraphPolicy, ColorMapPolicy, graph, u, colorMap);
                while (iterator.MoveNext())
                    yield return iterator.Current;
            }
        }

        private IEnumerator<DfsStep<TVertex>> EnumerateVerticesCore<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            Debug.Assert(vertices != null, "vertices != null");

            yield return new DfsStep<TVertex>(DfsStepKind.StartVertex, startVertex);
            var startIterator = new DfsVertexIterator<
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

                yield return new DfsStep<TVertex>(DfsStepKind.StartVertex, u);
                var vertexIterator = new DfsVertexIterator<
                    TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                    GraphPolicy, ColorMapPolicy, graph, u, colorMap);
                while (vertexIterator.MoveNext())
                    yield return vertexIterator.Current;
            }
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
