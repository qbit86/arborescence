namespace Arborescence.Traversal.Generic;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Misnomer;
using Specialized.Incidence;
using Xunit;
using Graph = Models.ListAdjacencyGraph<
    int,
    Int32Dictionary<
        System.Collections.Generic.List<int>,
        System.Collections.Generic.List<System.Collections.Generic.List<int>>>>;
using EdgeEnumerator = Models.IncidenceEnumerator<int, System.Collections.Generic.List<int>.Enumerator>;

public class QueueGenericSearchEnumerateVerticesTest
{
    private static void EnumerateVerticesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        int vertexCount = graph.MinVertexCount;
        if (vertexCount is 0)
            return;

        ConcurrentQueue<int> frontier = new();

        using Rist<int> eagerSteps = new(vertexCount);
        using Rist<int> enumerableSteps = new(vertexCount);
        var bfsHandler = CreateBfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.MinVertexCount < 3)
                return;

            int sourceCount = graph.MinVertexCount / 3;
            var sources = Enumerable.Range(0, sourceCount);

            // ReSharper disable PossibleMultipleEnumeration
            EagerBfs<Endpoints<int>, EdgeEnumerator>.Traverse(graph, sources, vertexCount, bfsHandler);
            var vertices = EnumerableGenericSearch<Endpoints<int>, EdgeEnumerator>.EnumerateVertices(
                graph, sources, frontier, vertexCount);
            // ReSharper restore PossibleMultipleEnumeration
            enumerableSteps.AddRange(vertices);
        }
        else
        {
            int source = graph.MinVertexCount >> 1;
            EagerBfs<Endpoints<int>, EdgeEnumerator>.Traverse(graph, source, vertexCount, bfsHandler);
            var vertices = EnumerableGenericSearch<Endpoints<int>, EdgeEnumerator>.EnumerateVertices(
                graph, source, frontier, vertexCount);
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

    private static BfsHandler<int, Endpoints<int>, Graph> CreateBfsHandler<TCollection>(TCollection discoveredVertices)
        where TCollection : ICollection<int>
    {
        if (discoveredVertices is null)
            throw new ArgumentNullException(nameof(discoveredVertices));

        BfsHandler<int, Endpoints<int>, Graph> result = new();
        result.DiscoverVertex += (_, v) => discoveredVertices.Add(v);
        return result;
    }

    [Theory]
    [ClassData(typeof(ListAdjacencyGraphCollection))]
    internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(ListAdjacencyGraphCollection))]
    internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, true);
}
