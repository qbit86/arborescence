namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Misnomer;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
    using Graph = Models.MutableIndexedIncidenceGraph;

    public sealed class RecursiveDfsTest
    {
        private InstantDfs<Graph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy> InstantDfs { get; }

        private RecursiveDfs<Graph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy> RecursiveDfs { get; }

        private void TraverseCore(Graph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount >= 0, "graph.VertexCount >= 0");

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            using var instantSteps = new Rist<(string, int)>(graph.VertexCount);
            DfsHandler<Graph, int, int> instantHandler = CreateDfsHandler(instantSteps);

            byte[] recursiveColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(recursiveColorMap, 0, recursiveColorMap.Length);
            using var recursiveSteps = new Rist<(string, int)>(graph.VertexCount);
            DfsHandler<Graph, int, int> recursiveHandler = CreateDfsHandler(recursiveSteps);

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

        private static DfsHandler<Graph, int, int> CreateDfsHandler(IList<(string, int)> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            var result = new DfsHandler<Graph, int, int>();
            result.StartVertex += (_, v) => steps.Add((nameof(result.OnStartVertex), v));
            result.DiscoverVertex += (_, v) => steps.Add((nameof(result.DiscoverVertex), v));
            result.FinishVertex += (_, v) => steps.Add((nameof(result.FinishVertex), v));
            result.TreeEdge += (_, e) => steps.Add((nameof(result.TreeEdge), e));
            result.BackEdge += (_, e) => steps.Add((nameof(result.BackEdge), e));
            result.ExamineEdge += (_, e) => steps.Add((nameof(result.ExamineEdge), e));
            result.ForwardOrCrossEdge += (_, e) => steps.Add((nameof(result.ForwardOrCrossEdge), e));
            result.FinishEdge += (_, e) => steps.Add((nameof(result.FinishEdge), e));
            return result;
        }

        [Theory]
        [ClassData(typeof(MutableIndexedGraphCollection))]
        internal void Traverse_SingleSource(GraphParameter<Graph> p)
        {
            TraverseCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(MutableIndexedGraphCollection))]
        internal void Traverse_MultipleSource(GraphParameter<Graph> p)
        {
            TraverseCore(p.Graph, true);
        }
    }
}
