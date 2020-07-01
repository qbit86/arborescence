namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Misnomer;
    using Models;
    using Traversal;
    using Workbench;
    using Xunit;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public sealed class RecursiveDfsTest
    {
        public RecursiveDfsTest()
        {
            InstantDfs = default;
            RecursiveDfs = default;
        }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private RecursiveDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            RecursiveDfs { get; }

        private void TraverseCore(AdjacencyListIncidenceGraph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            using var instantSteps = new Rist<(string, int)>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> instantHandler = CreateDfsHandler(instantSteps);

            byte[] recursiveColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(recursiveColorMap, 0, recursiveColorMap.Length);
            using var recursiveSteps = new Rist<(string, int)>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> recursiveHandler = CreateDfsHandler(recursiveSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);

                InstantDfs.Traverse(graph, sources, instantColorMap, instantHandler);
                RecursiveDfs.Traverse(graph, sources, recursiveColorMap, recursiveHandler);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                InstantDfs.Traverse(graph, source, instantColorMap, instantHandler);
                RecursiveDfs.Traverse(graph, source, recursiveColorMap, recursiveHandler);
            }

            // Assert

            int instantStepCount = instantSteps.Count;
            int recursiveStepCount = recursiveSteps.Count;
            Assert.Equal(instantStepCount, recursiveStepCount);

            int count = instantStepCount;
            for (int i = 0; i < count; ++i)
            {
                (string, int) instantStep = instantSteps[i];
                (string, int) recursiveStep = recursiveSteps[i];

                if (instantStep == recursiveStep)
                    continue;

                Assert.Equal(instantStep, recursiveStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(instantColorMap);
            ArrayPool<byte>.Shared.Return(recursiveColorMap);
        }

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(IList<(string, int)> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.StartVertex += (g, v) => steps.Add((nameof(result.OnStartVertex), v));
            result.DiscoverVertex += (g, v) => steps.Add((nameof(result.DiscoverVertex), v));
            result.FinishVertex += (g, v) => steps.Add((nameof(result.FinishVertex), v));
            result.TreeEdge += (g, e) => steps.Add((nameof(result.TreeEdge), e));
            result.BackEdge += (g, e) => steps.Add((nameof(result.BackEdge), e));
            result.ExamineEdge += (g, e) => steps.Add((nameof(result.ExamineEdge), e));
            result.ForwardOrCrossEdge += (g, e) => steps.Add((nameof(result.ForwardOrCrossEdge), e));
            result.FinishEdge += (g, e) => steps.Add((nameof(result.FinishEdge), e));
            return result;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(GraphCollection))]
        internal void Traverse_SingleSource(GraphParameter graphParameter)
        {
            TraverseCore(graphParameter.Graph, false);
        }

        [Theory]
        [ClassData(typeof(GraphCollection))]
        internal void Traverse_MultipleSource(GraphParameter graphParameter)
        {
            TraverseCore(graphParameter.Graph, true);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
