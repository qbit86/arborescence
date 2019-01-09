// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Traversal;
    using Traversal.Advanced;
    using Workbench;
    using static System.Diagnostics.Debug;
    using ColorMap = ArrayPrefix<Traversal.Color>;
    using StepMap = System.ArraySegment<Traversal.Advanced.DfsStepKind>;
    using ColorMapPolicy = Models.IndexedMapPolicy<Traversal.Color>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal static partial class Program
    {
        private static void Main()
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("03"))
            {
                var parser = new IndexedEdgeListParser();
                IEnumerable<SourceTargetPair<int>> edges = parser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();

            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount}, ");
            Console.WriteLine($"{nameof(graph.EdgeCount)}: {graph.EdgeCount}");

            var vertices = new IndexCollection(graph.VertexCount);
            var indexedMapPolicy = new ColorMapPolicy(graph.VertexCount);

            {
                var dfs = BaselineMultipleSourceDfs<AdjacencyListIncidenceGraph, int, int, IndexCollection,
                        IndexCollectionEnumerator, ArraySegmentEnumerator<int>, ColorMap, IndexedDfsStep>.Create(
                    default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy,
                    default(IndexCollectionEnumerablePolicy), default(IndexedDfsStepPolicy));

                IEnumerable<IndexedDfsStep> steps = dfs.Traverse(graph, vertices);
                StepMap vertexKinds = new StepMap(new DfsStepKind[graph.VertexCount]);
                StepMap edgeKinds = new StepMap(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Recursive DFS forest", Console.Out);
            }

            {
                var dfs = MultipleSourceDfs<AdjacencyListIncidenceGraph, int, int, IndexCollection,
                        IndexCollectionEnumerator, ArraySegmentEnumerator<int>, ColorMap, IndexedDfsStep>.Create(
                    default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy,
                    default(IndexCollectionEnumerablePolicy), default(IndexedDfsStepPolicy));

                var steps = dfs.Traverse(graph, vertices);
                StepMap vertexKinds = new StepMap(new DfsStepKind[graph.VertexCount]);
                StepMap edgeKinds = new StepMap(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Boost DFS forest", Console.Out);
            }
        }

        private static void FillEdgeKinds(IEnumerable<IndexedDfsStep> steps, StepMap vertexKinds, StepMap edgeKinds)
        {
            Assert(steps != null);

            foreach (var step in steps)
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
