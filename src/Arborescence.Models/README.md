# Models — Arborescence Graph Library

[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg?label=Models&logo=nuget)](https://nuget.org/packages/Arborescence.Models/)

This package provides a generic implementation of graph interfaces and collection concepts.

`ListAdjacencyGraph<TVertex, TVertexMultimap>` implements an adjacency graph as a dictionary that maps from a vertex to a list of its out-neighbors of type `List<TVertex>`.  
`ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>` implements an incidence graph as a dictionary that maps from a vertex to a list of its out-edges of type `List<TEdge>`.

## Basic usage

For example, we have four airports (_Omsk_, _London_, _Istanbul_, _Taipei_) and five unspecified flights connecting them:

```
  ┌───>───┐       ┌─>─┐
[LHR]─>─[IST]─>─[TPE] │
  └───<───┘       └───┘

[OMS]
```

We can create an adjacency graph as follows:

```csharp
Dictionary<string, List<string>> neighborsByVertex = new(4)
{
    ["OMS"] = new(),
    ["LHR"] = new() { "IST", "IST" },
    ["IST"] = new() { "LHR" }
    // Let's add "TPE" later.
};
ListAdjacencyGraph<string, Dictionary<string, List<string>>> graph =
    ListAdjacencyGraphFactory<string>.Create(neighborsByVertex);
graph.TryAddVertex("TPE");
graph.AddEdge("IST", "TPE");
graph.AddEdge("TPE", "TPE");

IncidenceEnumerator<string, List<string>.Enumerator> flightsFromIstanbul =
    graph.EnumerateOutEdges("IST");
while (flightsFromIstanbul.MoveNext())
    Console.WriteLine(flightsFromIstanbul.Current);
```

Expected output is:

    [IST, LHR]
    [IST, TPE]

Now let's look at the actual flights that connect these airports:

```
        BA676
  ┌───────>───────┐             EVA5288
  │     TK1980    │      TK24     ┌─>─┐
[LHR]─────>─────[IST]─────>─────[TPE] │
  └───────<───────┘               └───┘
        BA677

[OMS]
```

The following is one of the ways you can create an incident graph.
(For clarity, we represent flights as integers rather than strings.)

```csharp
ListIncidenceGraph<string, int, Dictionary<int, string>, Dictionary<string, List<int>>> graph =
    ListIncidenceGraphFactory<string, int>.Create();
_ = graph.TryAddVertex("OMS");
_ = graph.TryAddEdge(676, "LHR", "IST");
_ = graph.TryAddEdge(1980, "LHR", "IST");
_ = graph.TryAddEdge(677, "IST", "LHR");
_ = graph.TryAddEdge(24, "IST", "TPE");
_ = graph.TryAddEdge(5288, "TPE", "TPE");

List<int>.Enumerator flightsFromIstanbul =
    graph.EnumerateOutEdges("IST");
while (flightsFromIstanbul.MoveNext())
    Console.WriteLine(flightsFromIstanbul.Current);
```

Expected output is:

    677
    24

