# Product Overview

Arborescence is a generic .NET library for graph algorithms and data structures.
It provides efficient implementations for graph representation, traversal, and search operations.

## Core Purpose

- Generic graph structures that implement data-structure-agnostic interfaces
- Widely used graph traversal algorithms (BFS, DFS)
- Specialized implementations optimized for integer vertices from contiguous ranges
- Building blocks for creating various graph-based data structures and APIs

## Package Architecture

The library is organized into the following main packages:

- **Abstractions**: Interfaces and concepts for examining graphs and collections
- **Models**: Generic graph structures implementing the core interfaces
- **Models.Specialized**: Efficient adjacency/incidence graphs for integer vertices
- **Primitives**: Building blocks for data structures and APIs
- **Primitives.Specialized**: Efficient specializations for different vocabulary types
- **Search**: Search algorithms (Dijkstra, A*)
- **Traversal**: Graph traversal algorithms (BFS, DFS)
- **Traversal.Specialized**: Traversal algorithms specialized for integer vertices
