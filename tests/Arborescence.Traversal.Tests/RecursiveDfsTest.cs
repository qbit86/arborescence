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
        private EagerDfs<Graph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy> EagerDfs { get; }

        private RecursiveDfs<Graph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy> RecursiveDfs { get; }

        private void TraverseCore(Graph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            byte[] eagerColorMap = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(eagerColorMap, 0, eagerColorMap.Length);
            using var eagerSteps = new Rist<(string, int)>(Math.Max(graph.VertexCount, 1));
            DfsHandler<Graph, int, int> eagerHandler = CreateDfsHandler(eagerSteps);

            byte[] recursiveColorMap = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(recursiveColorMap, 0, recursiveColorMap.Length);
            using var recursiveSteps = new Rist<(string, int)>(Math.Max(graph.VertexCount, 1));
            DfsHandler<Graph, int, int> recursiveHandler = CreateDfsHandler(recursiveSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);

                EagerDfs.Traverse(graph, sources, eagerColorMap, eagerHandler);
                RecursiveDfs.Traverse(graph, sources, recursiveColorMap, recursiveHandler);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                EagerDfs.Traverse(graph, source, eagerColorMap, eagerHandler);
                RecursiveDfs.Traverse(graph, source, recursiveColorMap, recursiveHandler);
            }

            // Assert

            int eagerStepCount = eagerSteps.Count;
            int recursiveStepCount = recursiveSteps.Count;
            Assert.Equal(eagerStepCount, recursiveStepCount);

            int count = eagerStepCount;
            for (int i = 0; i < count; ++i)
            {
                (string, int) eagerStep = eagerSteps[i];
                (string, int) recursiveStep = recursiveSteps[i];

                if (eagerStep == recursiveStep)
                    continue;

                Assert.Equal(eagerStep, recursiveStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(eagerColorMap);
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
