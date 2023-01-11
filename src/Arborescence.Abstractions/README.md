# Abstractions — Arborescence Graph Library

[![Arborescence.Abstractions version](https://img.shields.io/nuget/v/Arborescence.Abstractions.svg?label=Abstractions&logo=nuget)](https://nuget.org/packages/Arborescence.Abstractions/)

This package provides abstractions for graph-related concepts.

The library treats the term _graph_ in a specific sense.
The closest analog in mathematics is the notion of a _quiver_ [1] — a directed graph [2] where loops and multiple edges between two vertices are allowed.

For example:
```
          0
       ┌──>──┐
       │     │
       │  1  │  3
(a)   (b)─>─(c)─>─(d)┐
       └──<──┘     └<┘
          2         4
```

Here common restrictions of _simple directed graphs_ are relaxed:
- parallel edges like 0 and 1 are permitted,
- antiparallel edges like 1 and 2 are also fine,
- nothing special about loops like edge 4,
- isolated vertices like _a_ are allowed too.

# Basic usage

```cs
public sealed class MyGraph :
    IGraph<MyNode, MyLink>,
    IForwardIncidence<MyNode, MyLink, IEnumerator<MyLink>>
{
    public bool TryGetTail(MyLink edge, out MyNode tail) => ...;

    public bool TryGetHead(MyLink edge, out MyNode head) => ...;

    public IEnumerator<MyLink> EnumerateOutEdges(MyNode vertex) => ...;
}
```

---

[1] Quiver  
    https://en.wikipedia.org/wiki/Quiver_(mathematics)

[2] Directed graph  
    https://en.wikipedia.org/wiki/Graph_(discrete_mathematics)#Directed_graph
