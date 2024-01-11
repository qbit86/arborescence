namespace Arborescence.Traversal.Dfs;

using System;
using System.Collections.Generic;
using Misnomer;
using Specialized.Incidence;
using Xunit;
using Graph = Models.ListIncidenceGraph<
    int,
    int,
    Int32Dictionary<int, System.Collections.Generic.List<int>>,
    System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>>>;
using EdgeEnumerator = System.Collections.Generic.List<int>.Enumerator;

public sealed class DfsEnumerateVerticesTest
{
    private static void EnumerateVerticesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        if (graph.MinVertexCount == 0)
            return;

        using Rist<int> eagerSteps = new(graph.MinVertexCount);
        using Rist<int> enumerableSteps = new(graph.MinVertexCount);
        DfsHandler<int, int, Graph> dfsHandler = CreateDfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.MinVertexCount < 3)
                return;

            int sourceCount = graph.MinVertexCount / 3;
            IndexEnumerator sources = new(sourceCount);
            EagerDfs<int, EdgeEnumerator>.Traverse(graph, sources, graph.MinVertexCount, dfsHandler);
            IEnumerable<int> vertices = EnumerableDfs<int, EdgeEnumerator>.EnumerateVertices(
                graph, sources, graph.MinVertexCount);
            enumerableSteps.AddRange(vertices);
        }
        else
        {
            int source = graph.MinVertexCount - 1;
            EagerDfs<int, EdgeEnumerator>.Traverse(graph, source, graph.MinVertexCount, dfsHandler);
            IEnumerable<int> vertices = EnumerableDfs<int, EdgeEnumerator>.EnumerateVertices(
                graph, source, graph.MinVertexCount);
            enumerableSteps.AddRange(vertices);
        }

        // Assert

        int eagerStepCount = eagerSteps.Count;
        int enumerableStepCount = enumerableSteps.Count;
        Assert.Equal(eagerStepCount, enumerableStepCount);

        int count = eagerStepCount;
        for (int i = 0; i < count; ++i)
        {
            int eagerStep = eagerSteps[i];
            int enumerableStep = enumerableSteps[i];

            if (eagerStep == enumerableStep)
                continue;

            Assert.Equal(eagerStep, enumerableStep);
        }
    }

    private static DfsHandler<int, int, Graph> CreateDfsHandler<TCollection>(TCollection steps)
        where TCollection : ICollection<int>
    {
        if (steps is null)
            throw new ArgumentNullException(nameof(steps));

        DfsHandler<int, int, Graph> result = new();
        result.DiscoverVertex += (_, v) => steps.Add(v);
        return result;
    }

    [Theory]
    [ClassData(typeof(ListIncidenceGraphCollection))]
    internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(ListIncidenceGraphCollection))]
    internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, true);
}
