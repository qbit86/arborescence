// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Traversal.Advanced;
    using static System.Diagnostics.Debug;
    using ColorMap = ArrayPrefix<Traversal.Color>;
    using StepMap = System.ArraySegment<Traversal.Advanced.DfsStepKind>;
    using ColorMapPolicy = Models.IndexedMapPolicy<Traversal.Color>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.JaggedAdjacencyListIncidenceGraph, ArrayPrefixEnumerator<int>>;

    internal static partial class Program
    {
        private static void Main()
        {
            const int vertexUpperBound = 10;
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexUpperBound, 1.5));

            Console.WriteLine($"{nameof(vertexUpperBound)}: {vertexUpperBound}, {nameof(edgeCount)}: {edgeCount}");

            var builder = new JaggedAdjacencyListIncidenceGraphBuilder(vertexUpperBound);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexUpperBound);
                int target = prng.Next(vertexUpperBound);
                builder.TryAdd(source, target, out _);
            }

            JaggedAdjacencyListIncidenceGraph graph = builder.ToGraph();

            var vertices = new IndexCollection(graph.VertexUpperBound);
            var indexedMapPolicy = new ColorMapPolicy(graph.VertexUpperBound);

            {
                var dfs = new BaselineMultipleSourceDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                    IndexCollection, IndexCollectionEnumerator, ArrayPrefixEnumerator<int>,
                    ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexCollectionEnumerablePolicy>(
                    default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy,
                    default(IndexCollectionEnumerablePolicy));

                IEnumerable<Step<DfsStepKind, int, int>> steps = dfs.Traverse(graph, vertices);
                StepMap vertexKinds = new StepMap(new DfsStepKind[graph.VertexUpperBound]);
                StepMap edgeKinds = new StepMap(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Recursive DFS forest", Console.Out);
            }

            {
                var dfs = new MultipleSourceDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                    IndexCollection, IndexCollectionEnumerator, ArrayPrefixEnumerator<int>,
                    ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexCollectionEnumerablePolicy>(
                    default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy,
                    default(IndexCollectionEnumerablePolicy));

                var steps = dfs.Traverse(graph, vertices);
                StepMap vertexKinds = new StepMap(new DfsStepKind[graph.VertexUpperBound]);
                StepMap edgeKinds = new StepMap(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Boost DFS forest", Console.Out);
            }
        }

        private static void FillEdgeKinds(IEnumerable<Step<DfsStepKind, int, int>> steps,
            StepMap vertexKinds, StepMap edgeKinds)
        {
            Assert(steps != null);

            foreach (Step<DfsStepKind, int, int> step in steps)
            {
                switch (step.Kind)
                {
                    case DfsStepKind.TreeEdge:
                    case DfsStepKind.BackEdge:
                    case DfsStepKind.ForwardOrCrossEdge:
                        edgeKinds[step.Edge] = step.Kind;
                        break;
                    case DfsStepKind.StartVertex:
                        vertexKinds[step.Vertex] = step.Kind;
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
