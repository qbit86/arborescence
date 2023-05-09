namespace Arborescence.Incidence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Specialized;
using Workbench;
using Xunit;

public sealed class Int32IncidenceGraphTests
{
    [Fact]
    internal void EnumerateOutEdges_ExistingVertex_ReturnsKnownEdges()
    {
        // Arrange
        using TextReader textReader = IndexedGraphs.GetTextReader("09");
        IEnumerable<Int32Endpoints> rawEdges = IndexedEdgeListParser.ParseEdges(textReader);
#if NET5_0_OR_GREATER
        var edges = rawEdges.Select(Transform).ToList();
#else
        Endpoints<int>[] edges = rawEdges.Select(Transform).ToArray();
#endif
        int vertex = Base32.Parse("p");
        var expectedEdges = new HashSet<int> { 6, 8, 23, 25 };

        static Endpoints<int> Transform(Int32Endpoints endpoints) => new(endpoints.Tail, endpoints.Head);

        // Act
        Int32FrozenIncidenceGraph graph = Int32FrozenIncidenceGraphFactory.FromEdges(edges);
        ArraySegment<int>.Enumerator edgeEnumerator = graph.EnumerateOutEdges(vertex);
        HashSet<int> actualEdges = new(4);
        while (edgeEnumerator.MoveNext())
            actualEdges.Add(edgeEnumerator.Current);

        // Assert
        Assert.Equal(expectedEdges, actualEdges);
    }
}
