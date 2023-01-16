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

We start with the simpler concept of an adjacency graph, where edges (flights) are of no interest, only connected vertices (airports).

```csharp
using NeighborEnumerator = System.ArraySegment<string>.Enumerator;

public sealed class FlightAdjacencyGraph :
    IAdjacency<string, NeighborEnumerator>
{
    private readonly Dictionary<string, string[]> _neighborsByAirport;

    private FlightAdjacencyGraph(
        Dictionary<string, string[]> neighborsByAirport) =>
        _neighborsByAirport = neighborsByAirport;

    public NeighborEnumerator EnumerateNeighbors(string vertex) =>
        _neighborsByAirport.TryGetValue(vertex, out string[]? neighbors)
            ? new ArraySegment<string>(neighbors).GetEnumerator()
            : ArraySegment<string>.Empty.GetEnumerator();

    public static FlightAdjacencyGraph Create()
    {
        Dictionary<string, string[]> neighborsByAirport = new(3)
        {
            { "LHR", new[] { "IST", "IST" } },
            { "IST", new[] { "LHR", "TPE" } },
            { "TPE", new[] { "TPE" } }
        };
        return new(neighborsByAirport);
    }
}
```

Where can we fly from Istanbul?

```csharp
var adjacencyGraph = FlightAdjacencyGraph.Create();
NeighborEnumerator istanbulNeighborEnumerator =
    adjacencyGraph.EnumerateNeighbors("IST");
while (istanbulNeighborEnumerator.MoveNext())
    Console.WriteLine(istanbulNeighborEnumerator.Current);
```

Expected output is:

```
LHR
TPE
```

The second example demonstrates an incidence graph.
Let's consider only the digits of the flight ids for simplicity, so we could encode them as `int`s: `676` instead of `BA676`, `1980` instead of `TK1980`, and so on.

```csharp
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

public sealed class FlightIncidenceGraph :
    IGraph<string, int>, IForwardIncidence<string, int, EdgeEnumerator>
{
    private readonly Dictionary<int, string> _destinationByFlight;
    private readonly Dictionary<string, int[]> _flightsByOrigin;
    private readonly Dictionary<int, string> _originByFlight;

    private FlightIncidenceGraph(
        Dictionary<int, string> originByFlight,
        Dictionary<int, string> destinationByFlight,
        Dictionary<string, int[]> flightsByOrigin)
    {
        _originByFlight = originByFlight;
        _destinationByFlight = destinationByFlight;
        _flightsByOrigin = flightsByOrigin;
    }

    public EdgeEnumerator EnumerateOutEdges(string vertex) =>
        _flightsByOrigin.TryGetValue(vertex, out int[]? flights)
            ? new ArraySegment<int>(flights).GetEnumerator()
            : ArraySegment<int>.Empty.GetEnumerator();

    public bool TryGetTail(int edge, [MaybeNullWhen(false)] out string tail) =>
        _originByFlight.TryGetValue(edge, out tail);

    public bool TryGetHead(int edge, [MaybeNullWhen(false)] out string head) =>
        _destinationByFlight.TryGetValue(edge, out head);

    public static FlightIncidenceGraph Create()
    {
        Dictionary<int, string> originByFlight = new(5)
        {
            { 676, "LHR" }, { 1980, "LHR" }, { 677, "IST" }, { 24, "IST" }, { 5288, "TPE" }
        };
        Dictionary<int, string> destinationByFlight = new(5)
        {
            { 676, "IST" }, { 1980, "IST" }, { 677, "LHR" }, { 24, "TPE" }, { 5288, "TPE" }
        };
        Dictionary<string, int[]> flightsByOrigin = new(3)
        {
            { "LHR", new[] { 676, 1980 } },
            { "IST", new[] { 677, 24 } },
            { "TPE", new[] { 5288 } }
        };
        return new(originByFlight, destinationByFlight, flightsByOrigin);
    }
}
```

Which flights are available from Istanbul?

```csharp
var incidenceGraph = FlightIncidenceGraph.Create();
EdgeEnumerator istanbulFlightEnumerator =
    incidenceGraph.EnumerateOutEdges("IST");
while (istanbulFlightEnumerator.MoveNext())
    Console.WriteLine(istanbulFlightEnumerator.Current);
```

Expected output is:

```
677
24
```

Which airports are connected by the flight `676`?

```csharp
if (incidenceGraph.TryGetTail(676, out string? flight676Origin))
    Console.WriteLine(flight676Origin);
if (incidenceGraph.TryGetHead(676, out string? flight676Destination))
    Console.WriteLine(flight676Destination);
```

Expected output is:

```
LHR
IST
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
