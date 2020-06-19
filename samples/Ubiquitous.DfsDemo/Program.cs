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
            var builder = new AdjacencyListIncidenceGraphBuilder(0, 31);

            using (TextReader textReader = IndexedGraphs.GetTextReader("08"))
            {
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

            InstantDfs<AdjacencyListIncidenceGraph, int, int, ArraySegmentEnumerator<int>, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy> dfs = default;

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var vertices = new IndexEnumerator(2);
            var vertexKinds = new DfsStepKind[graph.VertexCount];
            var edgeKinds = new DfsStepKind[graph.EdgeCount];
            DfsHandler<AdjacencyListIncidenceGraph, int, int> handler = CreateHandler(vertexKinds, edgeKinds);

            dfs.Traverse(graph, vertices, colorMap, handler);

            SerializeGraph(graph, vertexKinds, edgeKinds, "DFS forest", Console.Out);
            ArrayPool<byte>.Shared.Return(colorMap);
        }

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateHandler(
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
