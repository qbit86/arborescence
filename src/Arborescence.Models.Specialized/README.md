# Models.Specialized — Arborescence Graph Library

[![Arborescence.Models.Specialized version](https://img.shields.io/nuget/v/Arborescence.Models.Specialized.svg?label=Models.Specialized&logo=nuget)](https://nuget.org/packages/Arborescence.Models.Specialized/)

This package provides an efficient implementation of _Out-Edges Incidence_ and _Out-Neighbors Adjacency_ for a special case of integer vertices from contiguous range.

            ┌   tail : E → V?
    Graph   ┤
            └   head : E → V?       ┐
                                    ├   Forward Incidence
                out-edges : V → [E] ┘

                out-neighbors: V → [V]

`Int32AdjacencyGraph` represents a directed multigraph (permitting loops) with edges not having their own identity[^EWO].  
`Int32IncidenceGraph` represents a directed multigraph (permitting loops) with edges having their own identity[^EWI].

Vertices are represented as integers and must fill the range [0.._VertexCount_).  
Edges are stored as incidence lists in contiguous spans.

## Basic usage

Let's consider this simple graph:

           ┌──>──┐
    (0)   (1)─>─(2)─>─(3)┐
           └──<──┘     └<┘

This is how to recreate it in the code:

```csharp
Endpoints<int>[] edges =
{
    new(1, 2),
    new(1, 2),
    new(2, 1),
    new(2, 3),
    new(3, 3)
};
Int32AdjacencyGraph graph =
    Int32AdjacencyGraphFactory.FromEdges(edges);
Console.WriteLine(graph.VertexCount);
```

The expected output is `4` — including vertex 0, even if it was not specified when creating the graph.
The number of vertices is determined as one plus the maximum id of the vertices, so they fill the range [0.._VertexCount_).

Now let's explore the edges incident to vertex 2:

```csharp
const int vertex = 2;
var edgeEnumerator = graph.EnumerateOutEdges(vertex);
while (edgeEnumerator.MoveNext())
{
    var edge = edgeEnumerator.Current;
    Debug.Assert(graph.TryGetTail(edge, out int tail) &&
        tail == vertex);
    if (graph.TryGetHead(edge, out int head))
        Console.WriteLine(head);
}
```

The expected output — all the neighbors of vertex 2:

    1
    3

[^EWI]: https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)

[^EWO]: https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_without_own_identity)
