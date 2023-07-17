namespace Arborescence.Traversal.Dfs;

using System;
using System.Collections.Generic;
using Misnomer;
using Specialized.Incidence;
using Xunit;
using Graph = Models.Specialized.Int32IncidenceGraph;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

public sealed class DfsEnumerateEdgesTest
{
    private static void EnumerateEdgesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        int vertexCount = Math.Max(graph.VertexCount, 1);

        using Rist<int> eagerSteps = new(vertexCount);
        using Rist<int> enumerableSteps = new(vertexCount);
        DfsHandler<int, int, Graph> dfsHandler = CreateDfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IndexEnumerator sources = new(sourceCount);

            EagerDfs<int, EdgeEnumerator>.Traverse(graph, sources, vertexCount, dfsHandler);
            IEnumerable<int> edges = EnumerableDfs<int, EdgeEnumerator>.EnumerateEdges(graph, sources, vertexCount);
            enumerableSteps.AddRange(edges);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerDfs<int, EdgeEnumerator>.Traverse(graph, source, vertexCount, dfsHandler);
            IEnumerable<int> edges = EnumerableDfs<int, EdgeEnumerator>.EnumerateEdges(graph, source, vertexCount);
            enumerableSteps.AddRange(edges);
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

    private static DfsHandler<int, int, Graph> CreateDfsHandler(IList<int> treeEdges)
    {
        if (treeEdges is null)
            throw new ArgumentNullException(nameof(treeEdges));

        DfsHandler<int, int, Graph> result = new();
        result.TreeEdge += (_, e) => treeEdges.Add(e);
        return result;
    }

    [Theory]
    [ClassData(typeof(Int32IncidenceGraphCollection))]
    internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(Int32IncidenceGraphCollection))]
    internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, true);
}
