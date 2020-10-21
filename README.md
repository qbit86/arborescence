# Arborescence

[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg)](https://www.nuget.org/packages/Arborescence.Models/)

Arborescence is a generic .NET library for dealing with graphs.

API structure is inspired by [Concept C#] and [Boost Graph Concepts]. 

## Features

* [Abstractions]
* [Primitives]
* [Models]
* Algorithms
    * [Traversal]

## Basic usage

```cs
var builder = new SimpleIncidenceGraph.Builder();
builder.TryAdd(2, 0);
builder.TryAdd(3, 2);
builder.TryAdd(0, 3);
builder.TryAdd(3, 0);
SimpleIncidenceGraph graph = builder.ToGraph();

Bfs<SimpleIncidenceGraph, Endpoints, ArraySegment<Endpoints>.Enumerator> bfs;

IEnumerator<Endpoints> edges = bfs.EnumerateEdges(graph, graph.VertexCount, 2);
while (edges.MoveNext())
    Console.WriteLine(edges.Current);
```
```
[2, 0]
[0, 3]
```

## License

[MIT](LICENSE.txt)

The icon is designed by [OpenMoji](https://openmoji.org) — the open-source emoji and icon project. License: [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

[Abstractions]: https://www.nuget.org/packages/Arborescence.Abstractions/
[Boost Graph Concepts]: https://www.boost.org/doc/libs/1_74_0/libs/graph/doc/graph_concepts.html
[Concept C#]: https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md
[Models]: https://www.nuget.org/packages/Arborescence.Models/
[Primitives]: https://www.nuget.org/packages/Arborescence.Primitives/
[Traversal]: https://www.nuget.org/packages/Arborescence.Traversal/
