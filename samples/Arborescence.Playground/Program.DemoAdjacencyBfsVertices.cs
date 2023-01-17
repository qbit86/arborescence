namespace Arborescence;

using System;
using System.Collections.Generic;
using System.Linq;
using Traversal.Adjacency;

internal static partial class Program
{
    private static void DemoAdjacencyBfsVertices()
    {
        AdjacencyGraph adjacencyGraph = new();
        Node source = new(3);
        IEnumerator<Node> vertexEnumerator =
            EnumerableBfs<Node>.EnumerateVertices(adjacencyGraph, source);
        while (vertexEnumerator.MoveNext())
            Console.WriteLine(vertexEnumerator.Current);
    }
}

public readonly record struct Node(int Value);

public sealed class AdjacencyGraph : IAdjacency<Node, IEnumerator<Node>>
{
    public IEnumerator<Node> EnumerateNeighbors(Node vertex) =>
        vertex.Value is < 0 or >= 10
            ? Enumerable.Empty<Node>().GetEnumerator()
            : EnumerateNeighborsIterator(vertex);

    private static IEnumerator<Node> EnumerateNeighborsIterator(Node vertex)
    {
        yield return new(vertex.Value >> 1);
        yield return new(vertex.Value << 1);
    }
}
