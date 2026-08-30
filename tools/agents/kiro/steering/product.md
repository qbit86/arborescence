# Product Overview

Arborescence is a generic .NET library of graph data structures and algorithms.
The interfaces stay data-structure-agnostic, so a caller keeps its own graph representation.

## Packages

Every package name carries the `Arborescence.` prefix.

- **Abstractions** — interfaces and concepts for examining graphs and collections.
- **Primitives** — building blocks for data structures and APIs.
- **Models** — generic graph structures that implement the interfaces.
- **Traversal** — BFS and DFS.
- **Search** — Dijkstra; preview.
- **Primitives.Specialized**, **Models.Specialized**, **Traversal.Specialized** — `Int32` counterparts of the base package, for vertices and keys from a contiguous range.

**Traversal.Internal** is not a package.
It holds the internal generic implementation that Traversal and Traversal.Specialized share.
Both packages embed its assembly, so neither one depends on the other.

```mermaid
flowchart BT
    Abstractions
    Primitives
    Models
    PrimitivesSpecialized["Primitives.Specialized"]
    TraversalInternal["Traversal.Internal"]
    Traversal
    ModelsSpecialized["Models.Specialized"]
    TraversalSpecialized["Traversal.Specialized"]

    Models --> Abstractions
    Models --> Primitives
    PrimitivesSpecialized --> Primitives
    TraversalInternal --> Abstractions
    TraversalInternal --> Primitives
    Traversal --> Abstractions
    Traversal --> Primitives
    Traversal --> TraversalInternal
    ModelsSpecialized --> Abstractions
    ModelsSpecialized --> Models
    ModelsSpecialized --> PrimitivesSpecialized
    TraversalSpecialized --> PrimitivesSpecialized
    TraversalSpecialized --> Abstractions
    TraversalSpecialized --> Primitives
    TraversalSpecialized --> TraversalInternal

    classDef default fill:white,stroke:black,color:black
    classDef specialized fill:grey,stroke:black,color:black
    classDef internal fill:white,stroke:black,color:black,stroke-dasharray:4 3
    class PrimitivesSpecialized,ModelsSpecialized,TraversalSpecialized specialized
    class TraversalInternal internal
```
