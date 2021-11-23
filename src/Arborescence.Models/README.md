# Arborescence Graph Library

Undirected graphs are basically “duplicated” directed graphs,
but storage is more efficient than in case of naive edge duplication.

## Storage Layout

### Examples

#### Unordered directed graph

```plantuml
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

#### Sorted directed graph

```plantuml
digraph Sorted {
  node [shape=circle fontname="Times-Italic"]
  a -> b [xlabel=0]
  a -> b [xlabel=1]
  c -> c [xlabel=2]
  {
    rank=same
    b d
  }
}
```

### IndexedIncidenceGraph

Used in general case where graph is permitted to have parallel edges.

|         Length | Content             |
|---------------:|:--------------------|
|              1 | _vertexCount_ (_n_) |
|        2 × _n_ | _edgeBoundByVertex_ |
|            _m_ | _reorderedEdges_    |
|            _m_ | _heads_             |
|            _m_ | _tails_             |

```
vertexCount    reorderedEdges     tails
        ↓↓↓             ↓↓↓↓↓     ↓↓↓↓↓
        [4][_^|_^|_^|_^][021][bcb][aca]
           ↑↑↑↑↑↑↑↑↑↑↑↑↑     ↑↑↑↑↑
              edgeBounds     heads
```

### SimpleIncidenceGraph

|         Length | Content              |
|---------------:|:---------------------|
|              1 | _vertexCount_ (_n_)  |
|            _n_ | _UpperBoundByVertex_ |
|            _m_ | _edges_              |
