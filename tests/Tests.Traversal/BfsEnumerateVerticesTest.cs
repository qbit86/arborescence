namespace Arborescence.Traversal.Bfs;

using System;
using System.Collections.Generic;
using System.Linq;
using Misnomer;
using Specialized.Incidence;
using Xunit;
using Graph = Models.Specialized.Int32AdjacencyGraph;
using EdgeEnumerator = Models.IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

public sealed class BfsEnumerateVerticesTest
{
    private static void EnumerateVerticesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        int vertexCount = graph.VertexCount;
        if (vertexCount is 0)
            return;

        using Rist<int> eagerSteps = new(vertexCount);
        using Rist<int> enumerableSteps = new(vertexCount);
        BfsHandler<int, Endpoints<int>, Graph> bfsHandler = CreateBfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IEnumerable<int> sources = Enumerable.Range(0, sourceCount);

            // ReSharper disable PossibleMultipleEnumeration
            EagerBfs<Endpoints<int>, EdgeEnumerator>.Traverse(graph, sources, vertexCount, bfsHandler);
            IEnumerable<int> vertices = EnumerableBfs<Endpoints<int>, EdgeEnumerator>.EnumerateVertices(
                graph, sources, vertexCount);
            // ReSharper restore PossibleMultipleEnumeration
            enumerableSteps.AddRange(vertices);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerBfs<Endpoints<int>, EdgeEnumerator>.Traverse(graph, source, vertexCount, bfsHandler);
            IEnumerable<int> vertices = EnumerableBfs<Endpoints<int>, EdgeEnumerator>.EnumerateVertices(
                graph, source, vertexCount);
            enumerableSteps.AddRange(vertices);
        }

        // Assert

        int eagerStepCount = eagerSteps.Count;
        int enumerableStepCount = enumerableSteps.Count;
        Assert.Equal(eagerStepCount, enumerableStepCount);

        for (int i = 0; i < eagerStepCount; ++i)
        {
            int eagerStep = eagerSteps[i];
            int enumerableStep = enumerableSteps[i];
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
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, true);
}
