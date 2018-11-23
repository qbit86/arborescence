# Ubiquitous Graphs

## Data Layout

### AdjacencyList-IncidenceGraph

|            Length | Content       |
|------------------:|:--------------|
|                 1 | `vertexCount` |
| 2 × `vertexCount` | `edgeBounds`  |
|       `edgeCount` | `reorderedEdges` |
|       `edgeCount` | `targets`        |
|       `edgeCount` | `sources`        |

```
[4][··|··|··|··][···][···][···]
```

```plantuml
digraph G {
  node [shape=circle]
  b -> c [xlabel=0]
  d -> d [xlabel=1]
  b -> c [xlabel=2]
  {
    rank = same
    a, c
  }
}
```

### Sorted-AdjacencyList-IncidenceGraph

### EdgeList-IncidenceGraph
