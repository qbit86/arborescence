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

We can create the adjacency graph as follows:

```csharp
Dictionary<string, List<string>> neighborsByVertex = new(4)
{
    ["OMS"] = new(),
    ["LHR"] = new() { "IST", "IST" },
    ["IST"] = new() { "LHR" /* Let's add "TPE" later. */ },
    ["TPE"] = new() { "TPE" }
};
ListAdjacencyGraph<string, Dictionary<string, List<string>>> graph =
    ListAdjacencyGraphFactory<string>.Create(neighborsByVertex);
graph.AddEdge("IST", "TPE");

IncidenceEnumerator<string, List<string>.Enumerator> flightsFromIstanbul =
    graph.EnumerateOutEdges("IST");
while (flightsFromIstanbul.MoveNext())
    Console.WriteLine(flightsFromIstanbul.Current);
```

Expected output is:

    [IST, LHR]
    [IST, TPE]
