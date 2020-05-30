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
    using IndexedDfsStep = Traversal.DfsStep<int>;

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

            var dfs = MultipleSourceDfs<AdjacencyListIncidenceGraph, int, int, IndexCollection,
                IndexCollectionEnumerator, ArraySegmentEnumerator<int>, byte[], IndexedDfsStep>.Create(
                default(IndexedAdjacencyListGraphPolicy), default(IndexedColorMapPolicy),
                default(IndexCollectionEnumerablePolicy), default(IndexedDfsStepPolicy));

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var vertices = new IndexCollection(graph.VertexCount);
            var vertexKinds = new DfsStepKind[graph.VertexCount];
            var edgeKinds = new DfsStepKind[graph.EdgeCount];
            var steps = dfs.Traverse(graph, vertices, colorMap);

            FillStepKinds(steps, vertexKinds, edgeKinds);
            SerializeGraph(graph, vertexKinds, edgeKinds, "DFS forest", Console.Out);
            ArrayPool<byte>.Shared.Return(colorMap);
        }

        private static void FillStepKinds(
            IEnumerable<IndexedDfsStep> steps, Span<DfsStepKind> vertexKinds, Span<DfsStepKind> edgeKinds)
        {
            Assert(steps != null);

            foreach (IndexedDfsStep step in steps)
            {
                switch (step.Kind)
                {
                    case DfsStepKind.TreeEdge:
                    case DfsStepKind.BackEdge:
                    case DfsStepKind.ForwardOrCrossEdge:
                        edgeKinds[step.Value] = step.Kind;
                        break;
                    case DfsStepKind.StartVertex:
                        vertexKinds[step.Value] = step.Kind;
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
