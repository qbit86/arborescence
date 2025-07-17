# Product Overview

Arborescence is a generic .NET library for graph algorithms and data structures.
It provides efficient implementations for graph representation, traversal, and search operations.

## Core Purpose

- Generic graph structures that implement data-structure-agnostic interfaces
- Widely used graph traversal algorithms (BFS, DFS)
- Specialized implementations optimized for integer vertices from contiguous ranges
- Building blocks for creating various graph-based data structures and APIs

## Package Architecture

The library is organized into 7 main NuGet packages:

- **Abstractions**: Interfaces and concepts for examining graphs and collections
- **Models**: Generic graph structures implementing the core interfaces
- **Models.Specialized**: Efficient adjacency/incidence graphs for integer vertices
- **Primitives**: Building blocks for data structures and APIs
- **Primitives.Specialized**: Efficient specializations for different vocabulary types
- **Traversal**: Graph traversal algorithms (BFS, DFS)
- **Traversal.Specialized**: Traversal algorithms specialized for integer vertices

## Target Users

- .NET developers working with graph algorithms
- Applications requiring graph-based data modeling
- Performance-sensitive applications needing optimized graph operations
- Projects requiring path finding, network analysis, or dependency analysis
