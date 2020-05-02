namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Misnomer.Extensions;
    using Models;
    using Traversal;
    using Workbench;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal static partial class Program
    {
        private static void DemoEnumerateVertices()
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("03"))
            {
                IEnumerable<SourceTargetPair<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();

            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount}");

            var indexedMapPolicy = default(IndexedColorMapPolicy);
            var dfs = IterativeDfs<AdjacencyListIncidenceGraph, int, int,
                ArraySegmentEnumerator<int>, byte[]>.Create(default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy);

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var vertices = dfs.EnumerateVertices(graph, 0, colorMap);
            var discoveredVertices = vertices
                .Where(v => v.Kind == DfsStepKind.DiscoverVertex)
                .Select(v => Convert.ToChar('a' + v.Vertex)).ToRist();

            Console.WriteLine(string.Join(", ", discoveredVertices));

            ArrayPool<byte>.Shared.Return(colorMap);
        }
    }
}
