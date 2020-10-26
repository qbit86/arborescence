# Arborescence

[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg?label=Models&logo=nuget)](https://www.nuget.org/packages/Arborescence.Models/)
[![Arborescence.Traversal version](https://img.shields.io/nuget/v/Arborescence.Traversal.svg?label=Traversal&logo=nuget)](https://www.nuget.org/packages/Arborescence.Traversal/)

Arborescence is a generic .NET library for dealing with graphs.

API structure is inspired by [Concept C#] and [Boost Graph Concepts].

## Features

* [Abstractions] — concepts and policies for examining graphs and collections in a data-structure neutral way.
* [Models] — some particular graph structures implementing the aforementioned interfaces.
* [Primitives] — basic blocks for building different data structures.
* Algorithms:
    * [Traversal] — widely used algorithms for traversing graphs such as BFS and DFS.

## Installation

To install packages of this library with NuGet package manager follow the links above.

## Basic usage

Let's consider a simple directed graph and a breadth first tree on it:  
![](/assets/example.svg)

This is how you create a graph, instantiate an algorithm, and run it against the graph:
```cs
var builder = new SimpleIncidenceGraph.Builder();
builder.Add(2, 0);
builder.Add(4, 3);
builder.Add(0, 4);
builder.Add(3, 2);
builder.Add(4, 4);
builder.Add(0, 2);
builder.Add(2, 4);
SimpleIncidenceGraph graph = builder.ToGraph();

Bfs<SimpleIncidenceGraph, Endpoints, ArraySegment<Endpoints>.Enumerator> bfs;

IEnumerator<Endpoints> edges = bfs.EnumerateEdges(graph, graph.VertexCount, source: 3);
while (edges.MoveNext())
    Console.WriteLine(edges.Current);
```

Expected output:
```
[3, 2]
[2, 0]
[2, 4]
```

## Advanced usage

For more sophisticated examples examine [samples/](samples/) directory.

## License

[MIT](LICENSE.txt)

The icon is designed by [OpenMoji](https://openmoji.org) — the open-source emoji and icon project.
License: [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

[Abstractions]: https://www.nuget.org/packages/Arborescence.Abstractions/
[Boost Graph Concepts]: https://www.boost.org/doc/libs/1_74_0/libs/graph/doc/graph_concepts.html
[Concept C#]: https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md
[Models]: https://www.nuget.org/packages/Arborescence.Models/
[Primitives]: https://www.nuget.org/packages/Arborescence.Primitives/
[Traversal]: https://www.nuget.org/packages/Arborescence.Traversal/
