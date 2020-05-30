namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Traversal;
    using Workbench;
    using static System.Diagnostics.Debug;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal static partial class Program
    {
        private static CultureInfo F => CultureInfo.InvariantCulture;

        private static void Main()
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("03"))
            {
                IEnumerable<SourceTargetPair<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

            var dfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, ArraySegmentEnumerator<int>, byte[]>.Create(
                default(IndexedAdjacencyListGraphPolicy), default(IndexedColorMapPolicy));

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var vertices = new RangeEnumerator(0, graph.VertexCount);
            var vertexKinds = new DfsStepKind[graph.VertexCount];
            var edgeKinds = new DfsStepKind[graph.EdgeCount];
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(vertexKinds, edgeKinds);

            dfs.Traverse(graph, vertices, colorMap, dfsHandler);

            SerializeGraph(graph, vertexKinds, edgeKinds, "DFS forest", Console.Out);
            ArrayPool<byte>.Shared.Return(colorMap);
        }

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(
            DfsStepKind[] vertexKinds, DfsStepKind[] edgeKinds)
        {
            Assert(vertexKinds != null, "vertexKinds != null");
            Assert(edgeKinds != null, "edgeKinds != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.StartVertex += (_, v) => vertexKinds[v] = DfsStepKind.StartVertex;
            result.TreeEdge += (_, e) => edgeKinds[e] = DfsStepKind.TreeEdge;
            result.BackEdge += (_, e) => edgeKinds[e] = DfsStepKind.BackEdge;
            result.ForwardOrCrossEdge += (_, e) => edgeKinds[e] = DfsStepKind.ForwardOrCrossEdge;
            return result;
        }
    }
}
