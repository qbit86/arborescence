# Models — Arborescence Graph Library

[![Arborescence.Models version](https://img.shields.io/nuget/v/Arborescence.Models.svg?label=Models&logo=nuget)](https://nuget.org/packages/Arborescence.Models/)

This package provides a basic implementation for graph and traversable concepts.

```
        ┌   tail : E → V
Graph   ┤
        └   head : E → V        ┐
                                ├   Traversable
            out-edges : V → [E] ┘
```

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
ArraySegment<Endpoints>.Enumerator edgesEnumerator = graph.EnumerateOutEdges(vertex);
while (edgesEnumerator.MoveNext())
{
    Endpoints edge = edgesEnumerator.Current;
    Debug.Assert(graph.TryGetTail(edge, out int tail) && tail == vertex);
    if (graph.TryGetHead(edge, out int head))
        Console.WriteLine(head); // 2
}
```
