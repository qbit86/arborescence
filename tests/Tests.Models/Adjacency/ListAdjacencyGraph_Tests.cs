namespace Arborescence.Models;

using System.Collections.Generic;
using Xunit;

public sealed class ListAdjacencyGraph_Tests
{
    [Fact]
    public void TryAddVertex_ShouldIncreaseVertexCount()
    {
        ListAdjacencyGraph<string, Dictionary<string, List<string>>> graph = ListAdjacencyGraphFactory<string>.Create();
        Assert.True(graph.TryAddVertex("μηδέν"));
        Assert.True(graph.TryAddVertex("δύο"));
        Assert.Equal(2, graph.VertexCount);
    }
}
