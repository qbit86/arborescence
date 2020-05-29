namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

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

            ColorMapPolicy.Clear(colorMap);

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

        private IEnumerator<DfsStep<TVertex>> EnumerateVerticesCore<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            Debug.Assert(vertices != null, "vertices != null");

            ColorMapPolicy.Clear(colorMap);

            yield return new DfsStep<TVertex>(DfsStepKind.StartVertex, startVertex);
            var startVertexIterator = new DfsVertexIterator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                GraphPolicy, ColorMapPolicy, graph, startVertex, colorMap);
            while (startVertexIterator.MoveNext())
                yield return startVertexIterator.Current;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
