﻿namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using StepMap = IndexedDictionary<DfsStepKind, DfsStepKind[]>;
    using ColorMapFactory = IndexedDictionaryFactory<Color>;

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

            {
                var dfs = BaselineDfsFactory.WithGraph<IndexedAdjacencyListGraph>()
                    .WithVertex<int>().WithEdge<int>()
                    .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                    .WithColorMap<ColorMap>()
                    .WithVertexConcept<IndexedAdjacencyListGraphInstance>()
                    .WithEdgeConcept<IndexedAdjacencyListGraphInstance>()
                    .WithColorMapFactory<ColorMapFactory>()
                    .Create();

                var vertexEnumerator = vertices.GetConventionalEnumerator();
                var steps = dfs.Traverse(graph, vertexEnumerator);
                var vertexKinds = IndexedDictionary.Create(new DfsStepKind[graph.VertexCount]);
                var edgeKinds = IndexedDictionary.Create(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Recursive DFS Forest", Console.Out);
            }

            {
                var dfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                    ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                    IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                    ColorMapFactory,
                    ListFactory<IndexedAdjacencyListGraph,
                        DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>();

                var vertexEnumerator = vertices.GetConventionalEnumerator();
                var steps = dfs.Traverse(graph, vertexEnumerator);
                var vertexKinds = IndexedDictionary.Create(new DfsStepKind[graph.VertexCount]);
                var edgeKinds = IndexedDictionary.Create(new DfsStepKind[graph.EdgeCount]);
                FillEdgeKinds(steps, vertexKinds, edgeKinds);

                SerializeGraphByEdges(graph, vertexKinds, edgeKinds, "Boost DFS Forest", Console.Out);
            }
        }

        private static void FillEdgeKinds(IEnumerable<Step<DfsStepKind, int, int>> steps,
            StepMap vertexKinds, StepMap edgeKinds)
        {
            Assert(steps != null);

            foreach (var step in steps)
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
