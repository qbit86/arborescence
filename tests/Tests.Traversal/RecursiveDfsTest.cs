namespace Arborescence.Traversal.Dfs;

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

public sealed class RecursiveDfsTest
{
    private static void TraverseCore(Graph graph, bool multipleSource)
    {
        // Arrange

        int vertexCount = graph.VertexCount;
        if (vertexCount is 0)
            return;

        using Rist<(string, int)> eagerSteps = new(vertexCount);
        DfsHandler<int, int, Graph> eagerHandler = CreateDfsHandler(eagerSteps);

        using Rist<(string, int)> recursiveSteps = new(vertexCount);
        DfsHandler<int, int, Graph> recursiveHandler = CreateDfsHandler(recursiveSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IndexEnumerator sources = new(sourceCount);

            EagerDfs<int, EdgeEnumerator>.Traverse(graph, sources, vertexCount, eagerHandler);
            RecursiveDfs<int, EdgeEnumerator>.Traverse(graph, sources, vertexCount, recursiveHandler);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerDfs<int, EdgeEnumerator>.Traverse(graph, source, vertexCount, eagerHandler);
            RecursiveDfs<int, EdgeEnumerator>.Traverse(graph, source, vertexCount, recursiveHandler);
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
    }

    private static DfsHandler<int, int, Graph> CreateDfsHandler<TCollection>(TCollection steps)
        where TCollection : ICollection<(string, int)>
    {
        DfsHandler<int, int, Graph> result = new();
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
    [ClassData(typeof(ListIncidenceGraphCollection))]
    internal void Traverse_SingleSource(GraphParameter<Graph> p) => TraverseCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(ListIncidenceGraphCollection))]
    internal void Traverse_MultipleSource(GraphParameter<Graph> p) => TraverseCore(p.Graph, true);
}
