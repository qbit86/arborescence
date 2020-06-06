# Ubiquitous Graphs

Undirected graphs are basically “duplicated” directed graphs, but storage is more efficient than in case of naive edge duplication.

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

### AdjacencyList-IncidenceGraph

Used in general case where graph is permitted to have parallel edges.

|                 Length | Content            |
|-----------------------:|:-------------------|
|                      1 | _vertexCount_      |
|      2 × _vertexCount_ | _edgeBounds_       |
|            _edgeCount_ | _reorderedEdges_   |
|            _edgeCount_ | _heads_            |
|            _edgeCount_ | _tails_            |

```
vertexCount    reorderedEdges     tails
        ↓↓↓             ↓↓↓↓↓     ↓↓↓↓↓
        [4][_^|_^|_^|_^][021][bcb][aca]
           ↑↑↑↑↑↑↑↑↑↑↑↑↑     ↑↑↑↑↑
              edgeBounds     heads
```

### Sorted-AdjacencyList-IncidenceGraph

|             Length | Content            |
|-------------------:|:-------------------|
|                  1 | _vertexCount_      |
|      _vertexCount_ | _edgeUpperBounds_  |
|        _edgeCount_ | _heads_            |
|        _edgeCount_ | _orderedTails_     |

_edgeCount_ = (_length_ − 1 − _vertexCount_) / 2

```
vertexCount      heads
        ↓↓↓      ↓↓↓↓↓
        [4][^^^^][bbc][aac]
           ↑↑↑↑↑↑     ↑↑↑↑↑
  edgeUpperBounds     orderedTails
```

### EdgeList-IncidenceGraph

Used for simple graphs where edge is unambiguously determined by a pair of vertices.

_vertexCount_

|             Length | Content          |
|-------------------:|:-----------------|
|        _edgeCount_ | _reorderedEdges_ |
|      _vertexCount_ | _edgeBounds_     |

```
reorderedEdges
         ↓↓↓↓↓
         [aac][____]
         [bbc][^^^^]
              ↑↑↑↑↑↑
              edgeBounds
```

### Sorted-EdgeList-IncidenceGraph

|             Length | Content            |
|-------------------:|:-------------------|
|                  1 | _vertexCount_      |
|      _vertexCount_ | _edgeBounds_       |

```
vertexCount
        ↓↓↓
        [4][^^^^]
           ↑↑↑↑↑↑
           edgeBounds
```

|        Length | Content          |
|--------------:|:-----------------|
|   _edgeCount_ | _sortedEdges_    |

```
sortedEdges
      ↓↓↓↓↓
      [aac]
      [bbc]
```
