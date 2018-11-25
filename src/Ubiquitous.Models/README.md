# Ubiquitous Graphs

## Storage Layout

### Examples

#### Unordered directed graph

```plantuml
digraph Unordered {
  node [shape=circle]
  a -> b [xlabel=0]
  c -> c [xlabel=1]
  a -> b [xlabel=2]
  {
    rank=same
    b, d
  }
}
```

#### Sorted directed graph

```plantuml
digraph Sorted {
  node [shape=circle]
  a -> b [xlabel=0]
  a -> b [xlabel=1]
  c -> c [xlabel=2]
  {
    rank=same
    b, d
  }
}
```

### AdjacencyList-IncidenceGraph

|                 Length | Content            |
|-----------------------:|:-------------------|
|                      1 | _vertexUpperBound_ |
| 2 × _vertexUpperBound_ | _edgeBounds_       |
|            _edgeCount_ | _reorderedEdges_   |
|            _edgeCount_ | _targets_          |
|            _edgeCount_ | _sources_          |

```
vertexUpperBound    reorderedEdges     sources
             ↓↓↓             ↓↓↓↓↓     ↓↓↓↓↓
             [4][_^|_^|_^|_^][021][bcb][aca]
                ↑↑↑↑↑↑↑↑↑↑↑↑↑     ↑↑↑↑↑
                   edgeBounds     targets
```

### Sorted-AdjacencyList-IncidenceGraph

|             Length | Content            |
|-------------------:|:-------------------|
|                  1 | _vertexUpperBound_ |
| _vertexUpperBound_ | _edgeUpperBounds_  |
|        _edgeCount_ | _targets_          |
|        _edgeCount_ | _orderedSources_   |

_edgeCount_ = (_length_ − 1 − _vertexUpperBound_) / 2

```
vertexUpperBound      targets
             ↓↓↓      ↓↓↓↓↓
             [4][^^^^][bbc][aac]
                ↑↑↑↑↑↑     ↑↑↑↑↑
       edgeUpperBounds     orderedSources
```

### EdgeList-IncidenceGraph

_vertexUpperBound_

|             Length | Content          |
|-------------------:|:-----------------|
|        _edgeCount_ | _reorderedEdges_ |
| _vertexUpperBound_ | _edgeBounds_     |

```
reorderedEdges
         ↓↓↓↓↓
         [aca][____]
         [bcb][^^^^]
              ↑↑↑↑↑↑
              edgeBounds
```

### Sorted-EdgeList-IncidenceGraph

|             Length | Content            |
|-------------------:|:-------------------|
|                  1 | _vertexUpperBound_ |
| _vertexUpperBound_ | _edgeBounds_       |

```
vertexUpperBound
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
