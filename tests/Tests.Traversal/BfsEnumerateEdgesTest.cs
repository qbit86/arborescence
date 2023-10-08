namespace Arborescence.Traversal.Bfs;

using System;
using System.Collections.Generic;
using System.Linq;
using Misnomer;
using Specialized.Incidence;
using Xunit;
using Graph = Models.Specialized.Int32AdjacencyGraph;
using EdgeEnumerator = Models.IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

public sealed class BfsEnumerateEdgesTest
{
    private static void EnumerateEdgesCore(Models.Specialized.Int32AdjacencyGraph graph, bool multipleSource)
    {
        // Arrange

        int vertexCount = graph.VertexCount;
        if (vertexCount is 0)
            return;

        using Rist<Endpoints<int>> eagerSteps = new(vertexCount);
        using Rist<Endpoints<int>> enumerableSteps = new(vertexCount);
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
            IEnumerable<Endpoints<int>> edges = EnumerableBfs<Endpoints<int>, EdgeEnumerator>.EnumerateEdges(
                graph, sources, vertexCount);
            // ReSharper restore PossibleMultipleEnumeration
            enumerableSteps.AddRange(edges);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerBfs<Endpoints<int>, EdgeEnumerator>.Traverse(graph, source, vertexCount, bfsHandler);
            IEnumerable<Endpoints<int>> edges = EnumerableBfs<Endpoints<int>, EdgeEnumerator>.EnumerateEdges(
                graph, source, vertexCount);
            enumerableSteps.AddRange(edges);
        }

        // Assert

        int eagerStepCount = eagerSteps.Count;
        int enumerableStepCount = enumerableSteps.Count;
        Assert.Equal(eagerStepCount, enumerableStepCount);

        for (int i = 0; i < eagerStepCount; ++i)
        {
            Endpoints<int> eagerStep = eagerSteps[i];
            Endpoints<int> enumerableStep = enumerableSteps[i];

            if (eagerStep == enumerableStep)
                continue;

            Assert.Equal(eagerStep, enumerableStep);
        }
    }

    private static BfsHandler<int, Endpoints<int>, Graph> CreateBfsHandler<TCollection>(TCollection treeEdges)
        where TCollection : ICollection<Endpoints<int>>
    {
        if (treeEdges is null)
            throw new ArgumentNullException(nameof(treeEdges));

        BfsHandler<int, Endpoints<int>, Graph> result = new();
        result.TreeEdge += (_, e) => treeEdges.Add(e);
        return result;
    }

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, true);
}
