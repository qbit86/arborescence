namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using Misnomer;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
    using Graph = Models.MutableIndexedIncidenceGraph;

    public sealed class RecursiveDfsTest
    {
        private EagerDfs<Graph, int, int, EdgeEnumerator> EagerDfs { get; }

        private RecursiveDfs<Graph, int, int, EdgeEnumerator> RecursiveDfs { get; }

        private void TraverseCore(Graph graph, bool multipleSource)
        {
            // Arrange

            byte[] eagerColorMapBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(eagerColorMapBackingStore, 0, eagerColorMapBackingStore.Length);
            IndexedColorDictionary eagerColorMap = new(eagerColorMapBackingStore);
            using Rist<(string, int)> eagerSteps = new(Math.Max(graph.VertexCount, 1));
            DfsHandler<Graph, int, int> eagerHandler = CreateDfsHandler(eagerSteps);

            byte[] recursiveColorMapBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(recursiveColorMapBackingStore, 0, recursiveColorMapBackingStore.Length);
            IndexedColorDictionary recursiveColorMap = new(recursiveColorMapBackingStore);
            using Rist<(string, int)> recursiveSteps = new(Math.Max(graph.VertexCount, 1));
            DfsHandler<Graph, int, int> recursiveHandler = CreateDfsHandler(recursiveSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                IndexEnumerator sources = new(sourceCount);

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

            ArrayPool<byte>.Shared.Return(eagerColorMapBackingStore);
            ArrayPool<byte>.Shared.Return(recursiveColorMapBackingStore);
        }

        private static DfsHandler<Graph, int, int> CreateDfsHandler(ICollection<(string, int)> steps)
        {
            DfsHandler<Graph, int, int> result = new();
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
