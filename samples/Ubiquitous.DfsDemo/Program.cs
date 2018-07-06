// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Traversal.Advanced;
    using static System.Diagnostics.Debug;
    using LegacyColorMap = IndexedDictionary<Traversal.Advanced.Color, Traversal.Advanced.Color[]>;
    using ColorMap = System.ArraySegment<Traversal.Advanced.Color>;
    using StepMap = IndexedDictionary<Traversal.Advanced.DfsStepKind, Traversal.Advanced.DfsStepKind[]>;
    using LegacyColorMapFactory = IndexedDictionaryFactory<Traversal.Advanced.Color>;
    using ColorMapConcept = IndexedMapConcept<IndexedAdjacencyListGraph, Traversal.Advanced.Color>;

    internal static partial class Program
    {
        private static void Main()
        {
            const int vertexCount = 10;
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.5));

            Console.WriteLine($"{nameof(vertexCount)}: {vertexCount}, {nameof(edgeCount)}: {edgeCount}");

            var builder = new IndexedAdjacencyListGraphBuilder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexCount);
                int target = prng.Next(vertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }

            IndexedAdjacencyListGraph graph = builder.MoveToIndexedAdjacencyListGraph();

            var vertices = new RangeCollection(0, graph.VertexCount);
            var indexedMapConcept = new IndexedMapConcept<IndexedAdjacencyListGraph, Color>(graph.VertexCount);

            {
                var dfs = BaselineDfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                    .WithVertex<int>().WithEdge<int>()
                    .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                    .WithColorMap<ColorMap>()
                    .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                    .WithColorMapFactory(indexedMapConcept)
                    .Create();

                RangeCollection.Enumerator vertexEnumerator = vertices.GetConventionalEnumerator();
                IEnumerable<Step<DfsStepKind, int, int>> steps = dfs.Traverse(graph, vertexEnumerator);
                StepMap vertexKinds = IndexedDictionary.Create(new DfsStepKind[graph.VertexCount]);
                StepMap edgeKinds = IndexedDictionary.Create(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Recursive DFS Forest", Console.Out);
            }

            {
                var dfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                    .WithVertex<int>().WithEdge<int>()
                    .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                    .WithColorMap<LegacyColorMap>()
                    .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                    .WithColorMapFactory<LegacyColorMapFactory>()
                    .Create();

                RangeCollection.Enumerator vertexEnumerator = vertices.GetConventionalEnumerator();
                var steps = dfs.Traverse(graph, vertexEnumerator);
                StepMap vertexKinds = IndexedDictionary.Create(new DfsStepKind[graph.VertexCount]);
                StepMap edgeKinds = IndexedDictionary.Create(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Boost DFS Forest", Console.Out);
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
