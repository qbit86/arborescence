# Traversal — Arborescence Graph Library

[![Arborescence.Traversal version](https://img.shields.io/nuget/v/Arborescence.Traversal.svg?label=Traversal&logo=nuget)](https://nuget.org/packages/Arborescence.Traversal/)

This package provides basic traversal algorithms for incidence and adjacency graphs:

- generic search taking a frontier as a parameter,
- breadth-first search (BFS), which is a special case of generic search with FIFO-queue as the frontier,
- depth-first search (DFS), which is _not_ a special case of generic search with LIFO-stack as the frontier [1].

## Basic usage

Let's consider this implicit adjacency graph [2]:

```csharp
public readonly record struct Node(int Value);

public sealed class AdjacencyGraph : IOutNeighborsAdjacency<Node, IEnumerator<Node>>
{
    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex) =>
        vertex.Value is < 0 or >= 10
            ? Enumerable.Empty<Node>().GetEnumerator()
            : EnumerateNeighborsIterator(vertex);

    private static IEnumerator<Node> EnumerateNeighborsIterator(
        Node vertex)
    {
        yield return new(vertex.Value >> 1);
        yield return new(vertex.Value << 1);
    }
}
```

This is how to traverse it in a breadth-first manner with an adjacency version of the algorithm:

```csharp
AdjacencyGraph adjacencyGraph = new();
Node source = new(3);
IEnumerator<Node> vertexEnumerator =
    EnumerableBfs<Node>.EnumerateVertices(adjacencyGraph, source);
while (vertexEnumerator.MoveNext())
    Console.WriteLine(vertexEnumerator.Current);
```

[1] Stack-based graph traversal ≠ depth first search  
https://11011110.github.io/blog/2013/12/17/stack-based-graph-traversal.html

[2] Implicit graph  
https://en.wikipedia.org/wiki/Implicit_graph
