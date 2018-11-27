﻿// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Traversal.Advanced;
    using Workbench;
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
            var builder = new JaggedAdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("03"))
            {
                var parser = new IndexedEdgeListParser();
                IEnumerable<SourceTargetPair<int>> edges = parser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            JaggedAdjacencyListIncidenceGraph graph = builder.ToGraph();

            Console.Write($"{nameof(graph.VertexUpperBound)}: {graph.VertexUpperBound}, ");
            Console.WriteLine($"{nameof(graph.EdgeCount)}: {graph.EdgeCount}");

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
