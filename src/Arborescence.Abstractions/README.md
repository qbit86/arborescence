# Abstractions — Arborescence Graph Library

[![Arborescence.Abstractions version](https://img.shields.io/nuget/v/Arborescence.Abstractions.svg?label=Abstractions&logo=nuget)](https://nuget.org/packages/Arborescence.Abstractions/)

This package provides abstractions for graph-related concepts.

The library treats the term _graph_ in a specific sense.
The closest analog in mathematics is the notion of a _quiver_ [1] — a directed graph [2] where loops and multiple edges between two vertices are allowed.

Let's look at an example of four airports (_Omsk_, _London_, _Istanbul_, _Taipei_) and five flights between them:

```
        BA676
  ┌───────>───────┐             EVA5288
  │     TK1980    │      TK24     ┌─>─┐
[LHR]─────>─────[IST]─────>─────[TPE] │
  └───────<───────┘               └───┘
        BA677

[OMS]
```

The same in Graphviz notation:

```
digraph Flights {
  rankdir=LR
  node [shape=box fontname="Times-Italic"]
  OMS // Omsk
  LHR // London
  IST // Istanbul
  TPE // Taipei
  edge [fontname="Monospace"]
  LHR -> IST [label="BA676"]
  LHR -> IST [label="TK1980"]
  IST -> LHR [label="BA677"]
  IST -> TPE [label="TK24"]
  TPE -> TPE [label="EVA5288"]
}
```

Here common restrictions of _simple directed graphs_ are relaxed:
- parallel edges like `BA676` and `TK1980` are permitted,
- antiparallel edges like `TK1980` and `BA677` are also fine,
- nothing special about loops like edge `EVA5288` [3],
- isolated vertices like _OMS_ are allowed too.

The edges are _not_ treated as a set of ordered pairs of vertices.
Instead, they are described in terms of two _incidence_ functions _tail_ and _head_ mapping the edges to their endpoints.
In the example above, the _tail_ function is defined as { `BA676` ↦ _LHR_, `TK1980` ↦ _LHR_, `BA677` ↦ _IST_, `TK24` ↦ _IST_, `EVA5288` ↦ _TPE_ }.

There are two distinct notions of multiple edges:
- Without their own identity [4]: the identity of an edge is defined solely by the two vertices it connects.
    Let's ignore for now the flight ids in the figure above.
    Then outgoing edges of vertex _LHR_ would be two entries of the same endpoints pair: ⟨_LHR_, _IST_⟩ and ⟨_LHR_, _IST_⟩ again.
- With their own identity [5]: edges are primitive entities just like vertices.
In this case, the outgoing edges of vertex _LHR_ are two different independent edges `BA676` and `TK1980`, which just occasionally happen to have the same endpoints.

Another useful function maps the vertex to its _outgoing edges_, making a graph traversable.
It must be consistent with the incidence function:    
∀_v_ ∀_e_   _e_ ∈ _out-edges_(_v_) ⇔ _v_ = _tail_(_e_)   

Together these functions form a scheme:
```
        ┌   tail : E → V?
Graph   ┤
        └   head : E → V?       ┐
                                ├   Forward Incidence
            out-edges : V → [E] ┘
```

The _adjacency_ concept provides an interface for access of the adjacent vertices (neighbors) to a vertex in a graph.
In some contexts there is only concern for the vertices, while the edges are not important.

## Basic usage

```cs
public sealed class MyIncidenceNetwork :
    IGraph<MyNode, MyLink>,
    IForwardIncidence<MyNode, MyLink, IEnumerator<MyLink>>
{
    public bool TryGetTail(MyLink edge, out MyNode tail) => ...

    public bool TryGetHead(MyLink edge, out MyNode head) => ...

    public IEnumerator<MyLink> EnumerateOutEdges(MyNode vertex) => ...
}
```

```cs
public sealed class MyAdjacencyNetwork :
    IAdjacency<MyNode, IEnumerator<MyNode>>
{
    public IEnumerator<MyNode> EnumerateNeighbors(
        MyNode vertex) => ...
}
```

---

[1] Quiver  
    https://en.wikipedia.org/wiki/Quiver_(mathematics)

[2] Directed graph  
    https://en.wikipedia.org/wiki/Graph_(discrete_mathematics)#Directed_graph

[3] EVA Air introduces special flight to nowhere  
    https://edition.cnn.com/travel/article/eva-air-hello-kitty-fathers-day-flight/index.html  
    https://flightaware.com/live/flight/EVA5288

[4] Edges without own identity  
    https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_without_own_identity)

[5] Edges with own identity  
    https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)
