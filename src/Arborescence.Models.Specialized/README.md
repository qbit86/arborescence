# Models — Arborescence Graph Library

[![Arborescence.Models.Specialized version](https://img.shields.io/nuget/v/Arborescence.Models.Specialized.svg?label=Models.Specialized&logo=nuget)](https://nuget.org/packages/Arborescence.Models.Specialized/)

This package provides a basic implementation for _Graph_ and _Forward Incidence_ concepts.

```
        ┌   tail : E → V?
Graph   ┤
        └   head : E → V?       ┐
                                ├   Forward Incidence
            out-edges : V → [E] ┘
```

`SimpleIncidenceGraph` represents a directed multigraph (permitting loops) with edges not having their own identity [1].  
`IndexedIncidenceGraph` represents a directed multigraph (permitting loops) with edges having their own identity [2].  
`MutableUndirectedSimpleIncidenceGraph` and `MutableUndirectedIndexedIncidenceGraph` provide their mutable undirected counterparts.

Vertices are represented as integers and must fill the range [0.._VertexCount_).  
Edges are stored as incidence lists in contiguous spans.

## Basic usage

Let's consider this simple graph:

```
       ┌──>──┐
(0)   (1)─>─(2)─>─(3)┐
       └──<──┘     └<┘
```

This is how to recreate it in the code:

```csharp
SimpleIncidenceGraph.Builder builder = new();
builder.Add(1, 2);
builder.Add(1, 2);
builder.Add(2, 1);
builder.Add(2, 3);
builder.Add(3, 3);
SimpleIncidenceGraph graph = builder.ToGraph();
Console.WriteLine(graph.VertexCount);
```

The expected output is `4` — including vertex 0 even if it was not mentioned while creating the graph.
Vertex count is determined as one plus the max id of the vertices, so they fill the range [0.._VertexCount_).

Now let's explore the edges incident to vertex 2:

```csharp
const int vertex = 2;
var edgeEnumerator = graph.EnumerateOutEdges(vertex);
while (edgeEnumerator.MoveNext())
{
    Endpoints edge = edgeEnumerator.Current;
    Debug.Assert(graph.TryGetTail(edge, out int tail) &&
        tail == vertex);
    if (graph.TryGetHead(edge, out int head))
        Console.WriteLine(head);
}
```

The expected output — all the neighbors of vertex 2:

```
1
3
```

---

[1] https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_without_own_identity)

[2] https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)
