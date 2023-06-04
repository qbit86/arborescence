# Models — Implementation

Undirected graphs are basically “duplicated” directed graphs.
However, for incidence graphs, storage can be more efficient than in the case of naive edge duplication.
For example, for every integer edge `e` we can consider the inverted edge `~e`.

## Storage Layout

### Examples

```
digraph Unordered {
  node [shape=circle fontname="Times-Italic"]
  a -> b [xlabel=0]
  c -> c [xlabel=1]
  a -> b [xlabel=2]
  {
    rank=same
    b d
  }
}
```

### Int32AdjacencyGraph

Offset       | Length    | Content
-------------|-----------|--------
0            | 1         | n = vertexCount
1            | 1         | m = edgeCount
2            | n         | upperBoundByVertex
2 + n        | m         | neighborsOrderedByTail

### Int32IncidenceGraph

Used in the general case where the graph is allowed to have parallel edges.

| Offset       | Length    | Content
|--------------|-----------|--------
| 0            | 1         | n = vertexCount
| 1            | 1         | m = edgeCount
| 2            | n         | upperBoundByVertex
| 2 + n        | m         | edgesOrderedByTail
| 2 + n + m    | m         | headByEdge
| 2 + n + 2m   | m         | tailByEdge

```
vertexCount  edgeCount   edgesOrderedByTail
          ↓  ↓           ↓↓↓↓↓
         [4][3][2|2|3|3][0|2|1][b|c|b][a|c|a]
                ↑↑↑↑↑↑↑         ↑↑↑↑↑  ↑↑↑↑↑
     upperBoundByVertex    headByEdge  tailByEdge
```
