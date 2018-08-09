// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Traversal.Advanced;
    using static System.Diagnostics.Debug;
    using ColorMap = System.ArraySegment<Traversal.Advanced.Color>;
    using StepMap = System.ArraySegment<Traversal.Advanced.DfsStepKind>;
    using ColorMapConcept = IndexedMapConcept<Traversal.Advanced.Color>;

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
            var indexedMapConcept = new ColorMapConcept(graph.VertexCount);

            {
                var dfs = BaselineDfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                    .WithVertex<int>().WithEdge<int>()
                    .WithEdgeEnumerator<List<int>.Enumerator>()
                    .WithColorMap<ColorMap>()
                    .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                    .WithColorMapConcept(indexedMapConcept)
                    .Create();

                RangeCollection.Enumerator vertexEnumerator = vertices.GetConventionalEnumerator();
                IEnumerable<Step<DfsStepKind, int, int>> steps = dfs.Traverse(graph, vertexEnumerator);
                StepMap vertexKinds = new StepMap(new DfsStepKind[graph.VertexCount]);
                StepMap edgeKinds = new StepMap(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Recursive DFS Forest", Console.Out);
            }

            {
                var dfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                    .WithVertex<int>().WithEdge<int>()
                    .WithEdgeEnumerator<List<int>.Enumerator>()
                    .WithColorMap<ColorMap>()
                    .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                    .WithColorMapConcept(indexedMapConcept)
                    .Create();

                RangeCollection.Enumerator vertexEnumerator = vertices.GetConventionalEnumerator();
                var steps = dfs.Traverse(graph, vertexEnumerator);
                StepMap vertexKinds = new StepMap(new DfsStepKind[graph.VertexCount]);
                StepMap edgeKinds = new StepMap(new DfsStepKind[graph.EdgeCount]);
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
