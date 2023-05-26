# Arborescence

[![Arborescence.Abstractions version](https://img.shields.io/nuget/v/Arborescence.Abstractions.svg?label=Abstractions&logo=nuget)](https://nuget.org/packages/Arborescence.Abstractions/)
[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg?label=Models&logo=nuget)](https://nuget.org/packages/Arborescence.Models/)
[![Arborescence.Primitives version](https://img.shields.io/nuget/v/Arborescence.Primitives.svg?label=Primitives&logo=nuget)](https://nuget.org/packages/Arborescence.Primitives/)
[![Arborescence.Traversal version](https://img.shields.io/nuget/v/Arborescence.Traversal.svg?label=Traversal&logo=nuget)](https://nuget.org/packages/Arborescence.Traversal/)

Arborescence is a generic .NET library for dealing with graphs.

## Features

- [Abstractions] — interfaces and concepts for examining graphs and collections in a data-structure neutral way.
- [Models] — some particular graph structures implementing the aforementioned interfaces.
- [Primitives] — basic blocks for building different data structures.
- Algorithms:
    - [Traversal] — widely used algorithms for traversing graphs such as BFS and DFS.

## Installation

To install packages of this library with NuGet package manager follow the links above.

## Basic usage

Let's consider a simple directed graph and a breadth-first tree on it:  
![](/assets/example.svg)

This is how you create a graph, instantiate an algorithm, and run it against the graph:

```csharp
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
Int32AdjacencyGraph graph = Int32AdjacencyGraphFactory.FromEdges(edges);

EnumerableBfs<
    Int32AdjacencyGraph,
    Endpoints<int>,
    IncidenceEnumerator<int, ArraySegment<int>.Enumerator>> bfs;

using IEnumerator<Endpoints<int>> treeEdges =
    bfs.EnumerateEdges(graph, source: 3, vertexCount: graph.VertexCount);
while (treeEdges.MoveNext())
    Console.WriteLine(treeEdges.Current);
```

Expected output:

    [3, 2]
    [2, 0]
    [2, 4]

## Advanced usage

For more sophisticated examples examine [samples/](samples) directory.

## License

[![License](https://img.shields.io/github/license/qbit86/arborescence)](LICENSE.txt)

The icon is designed by [OpenMoji](https://openmoji.org) — the open-source emoji and icon project.
License: [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

[Abstractions]: https://nuget.org/packages/Arborescence.Abstractions/

[Models]: https://nuget.org/packages/Arborescence.Models/

[Models.Specialized]: https://nuget.org/packages/Arborescence.Models.Specialized/

[Primitives]: https://nuget.org/packages/Arborescence.Primitives/

[Traversal]: https://nuget.org/packages/Arborescence.Traversal/
