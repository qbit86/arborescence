# Traversal.Specialized — Arborescence Graph Library

[![Arborescence.Traversal.Specialized version](https://img.shields.io/nuget/v/Arborescence.Traversal.Specialized.svg?label=Traversal.Specialized&logo=nuget)](https://nuget.org/packages/Arborescence.Traversal.Specialized/)

This package provides basic traversal algorithms for a special case of integer vertices from a contiguous range.
Traversal algorithms inherently use some auxiliary data structures, such as associative arrays and sets, to store explored nodes.
And when a set of vertices maps to the range of [0.._VertexCount_), then these data structures can be effectively represented as plain arrays rather than hash tables or balanced trees.

## Basic usage

```csharp
using Traversal.Specialized.Adjacency;

...

Endpoints<int>[] edges =
{
    new(2, 0),
    new(4, 3),
    new(0, 4),
    new(3, 2),
    new(4, 4),
    new(0, 2),
    new(2, 4)
};
var graph = Int32AdjacencyGraph.FromEdges(edges);

IEnumerable<Endpoints<int>> treeEdges =
    EnumerableBfs<ArraySegment<int>.Enumerator>.EnumerateEdges(
        graph, source: 3, graph.VertexCount);
foreach (Endpoints<int> edge in treeEdges)
    Console.WriteLine(edge);
```

Expected output:

    [3, 2]
    [2, 0]
    [2, 4]
