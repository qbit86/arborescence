# Models — Arborescence Graph Library

[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg?label=Models&logo=nuget)](https://nuget.org/packages/Arborescence.Models/)

This package provides a basic implementation for _Graph_ and _Traversable_ concepts.

```
        ┌   tail : E → V
Graph   ┤
        └   head : E → V        ┐
                                ├   Traversable
            out-edges : V → [E] ┘
```

`SimpleIncidenceGraph` represents a directed multigraph (permitting loops) with edges not having their own identity [1].  
`IndexedIncidenceGraph` represents a directed multigraph (permitting loops) with edges having their own identity [2].  
`MutableUndirectedSimpleIncidenceGraph` and `MutableUndirectedIndexedIncidenceGraph` provide their mutable undirected counterparts.

Vertices are represented as integers and must fill the range [0.._VertexCount_).  
Edges are stored as incidence lists in contiguous spans.

## Basic usage

```cs
SimpleIncidenceGraph.Builder builder = new();
builder.Add(2, 0);
builder.Add(4, 3);
builder.Add(0, 4);
builder.Add(3, 2);
builder.Add(4, 4);
builder.Add(0, 2);
builder.Add(2, 4);
SimpleIncidenceGraph graph = builder.ToGraph();

const int vertex = 3;
var edgesEnumerator = graph.EnumerateOutEdges(vertex);
while (edgesEnumerator.MoveNext())
{
    Endpoints edge = edgesEnumerator.Current;
    Debug.Assert(graph.TryGetTail(edge, out int tail) && tail == vertex);
    if (graph.TryGetHead(edge, out int head))
        Console.WriteLine(head); // 2
}
```

[1] https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_without_own_identity)

[2] https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)
