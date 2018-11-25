# Ubiquitous Graphs

## Data Layout

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
[4][↓↑|↓↑|↓↑|↓↑][021][bcb][aca]
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
vertedUpperBound      targets
             ↓↓↓      ↓↓↓↓↓
             [4][····][bbc][aac]
                ↑↑↑↑↑↑     ↑↑↑↑↑
       edgeUpperBounds     orderedSources
```

### EdgeList-IncidenceGraph

_vertexUpperBound_

|             Length | Content          |
|-------------------:|:-----------------|
| _vertexUpperBound_ | _edgeBounds_     |
|        _edgeCount_ | _reorderedEdges_ |

```
[↓↓↓↓][aca]
[↑↑↑↑][bcb]
```

### Sorted-EdgeList-IncidenceGraph

|             Length | Content            |
|-------------------:|:-------------------|
|                  1 | _vertexUpperBound_ |
| _vertexUpperBound_ | _edgeBounds_       |

```
[4][↑↑↑↑]
```

|        Length | Content          |
|--------------:|:-----------------|
|   _edgeCount_ | _sortedEdges_    |

```
[aac]
[bbc]
```
