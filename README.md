# Arborescence

[![Arborescence.Abstractions version](https://img.shields.io/nuget/v/Arborescence.Abstractions.svg?label=Abstractions&logo=nuget)](https://nuget.org/packages/Arborescence.Abstractions/)
[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg?label=Models&logo=nuget)](https://nuget.org/packages/Arborescence.Models/)
[![Arborescence.Models.Specialized version](https://img.shields.io/nuget/v/Arborescence.Models.Specialized.svg?label=Models.Specialized&logo=nuget)](https://nuget.org/packages/Arborescence.Models.Specialized/)
[![Arborescence.Primitives version](https://img.shields.io/nuget/v/Arborescence.Primitives.svg?label=Primitives&logo=nuget)](https://nuget.org/packages/Arborescence.Primitives/)
[![Arborescence.Primitives.Specialized version](https://img.shields.io/nuget/v/Arborescence.Primitives.Specialized.svg?label=Primitives.Specialized&logo=nuget)](https://nuget.org/packages/Arborescence.Primitives.Specialized/)
[![Arborescence.Traversal version](https://img.shields.io/nuget/v/Arborescence.Traversal.svg?label=Traversal&logo=nuget)](https://nuget.org/packages/Arborescence.Traversal/)

Arborescence is a generic .NET library for dealing with graphs.

## Features

- [Abstractions] — interfaces and concepts for examining graphs and collections in a data-structure-agnostic way.
- [Models] — generic graph structures that implement the aforementioned interfaces.
- [Models.Specialized] — special adjacency and incidence graph data structures that provide efficient implementation when vertices are integers from contiguous range.
- [Primitives] — building blocks for creating various data structures and APIs.
- [Primitives.Specialized] — efficient specializations for different vocabulary generic types.
- [Traversal] — widely used graph traversal algorithms such as BFS and DFS.

## Installation

To install packages of this library using the NuGet package manager, follow the links above.

## Basic usage

Let's consider a simple directed graph and a breadth-first tree on it:  
![](/assets/example.svg)

This is how you create a graph and run an algorithm against the graph:

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
Int32AdjacencyGraph graph =
    Int32AdjacencyGraphFactory.FromEdges(edges);

IEnumerable<Endpoints<int>> treeEdges =
    EnumerableBfs<int, ArraySegment<int>.Enumerator>.EnumerateEdges(
        graph, source: 3);
foreach (Endpoints<int> edge in treeEdges)
    Console.WriteLine(edge);
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

[Primitives.Specialized]: https://nuget.org/packages/Arborescence.Primitives.Specialized/

[Traversal]: https://nuget.org/packages/Arborescence.Traversal/
