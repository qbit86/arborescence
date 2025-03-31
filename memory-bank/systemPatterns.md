# System Patterns

## Architecture Overview

Arborescence follows a layered architecture with clearly separated concerns. The architecture can be visualized as follows:

```mermaid
graph BT
    subgraph Generic
        a[Abstractions]
        b[Primitives]
        c[Models]
        d[Traversal]
        e[Search]
    end

    subgraph Specialized
        f["Primitives.Specialized"]
        g["Models.Specialized"]
        h["Traversal.Specialized"]
    end

    style f fill:#f5f5f5,stroke:#333,stroke-width:1px
    style g fill:#f5f5f5,stroke:#333,stroke-width:1px
    style h fill:#f5f5f5,stroke:#333,stroke-width:1px

    c --> a
    c --> b
    d --> a
    d --> b
    e --> a
    e --> b
    f --> b
    g --> a
    g --> c
    g --> f
    h --> a
    h --> b
    h --> d
    h --> f
```

## Key Technical Decisions

### Layered Design

The system follows a clear layering pattern:
1. **Abstractions**: Foundation layer with interfaces and conceptual definitions
2. **Primitives**: Low-level data structures and utilities
3. **Models**: Graph data model implementations
4. **Traversal & Search**: Algorithms built on top of the lower layers

### Specialization Pattern

- Core packages provide general-purpose implementations
- Specialized packages optimize for specific use cases or performance
- Specialized implementations depend on both abstractions and their general-purpose counterparts

### Dependency Direction

- Higher-level modules depend on lower-level ones
- Abstractions do not depend on concrete implementations
- Specialized implementations depend on their general counterparts

## Component Relationships

### Graph Data Structure Components

- **Adjacency vs. Incidence**: The library appears to support both adjacency list and incidence list representations
- **Vertices & Edges**: Primitives define basic vertex and edge concepts
- **Collections**: Specialized collections for efficient graph operations

### Algorithm Components

- **Traversal**: BFS, DFS implementations with various strategies (Eager, Enumerable, Recursive)
- **Search**: Path-finding algorithms likely built on traversal foundations
- **Handlers**: Callback-based processing for vertices and edges during traversal
