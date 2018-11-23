# Ubiquitous Graphs

## Data Layout

### AdjacencyList-IncidenceGraph

|            Length | Content          |
|------------------:|:-----------------|
|                 1 | `vertexCount`    |
| 2 × `vertexCount` | `edgeBounds`     |
|       `edgeCount` | `reorderedEdges` |
|       `edgeCount` | `targets`        |
|       `edgeCount` | `sources`        |

```
[4][↓↑|↓↑|↓↑|↓↑][021][bcb][aca]
```

```plantuml
digraph G {
  node [shape=circle]
  a -> b [xlabel=0]
  c -> c [xlabel=1]
  a -> b [xlabel=2]
  {
    rank = same
    b, d
  }
}
```

### Sorted-AdjacencyList-IncidenceGraph

### EdgeList-IncidenceGraph
