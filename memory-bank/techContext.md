# Technical Context

## Technologies Used

- **.NET**: The project is built on the .NET platform
- **C#**: Primary programming language used
- **MSBuild**: Project system with .csproj files
- **Graphviz/DOT**: Used for visualizing graph structures (found in tools/Tools.Workbench/IndexedGraphs)

## Project Structure

The solution follows a modular structure with clear separation of concerns:

### Core Libraries

- **Arborescence.Abstractions**: Core interfaces and concepts
- **Arborescence.Primitives**: Basic data structures and utilities
- **Arborescence.Models**: Implementation of graph data models
- **Arborescence.Traversal**: Algorithms for graph traversal (BFS, DFS)
- **Arborescence.Search**: Search algorithms (Dijkstra, A*)

### Specialized Libraries

- **Arborescence.Primitives.Specialized**: Optimized primitive implementations
- **Arborescence.Models.Specialized**: Specialized graph model implementations
- **Arborescence.Traversal.Specialized**: Performance-optimized traversal algorithms

### Testing & Development

- **benchmarks**: Performance benchmarking code
- **tests**: Unit tests for various components
- **samples**: Example code demonstrating library usage
- **tools**: Development utilities including visualization tools

## Development Setup

- Uses modern .NET tooling with SDK-style projects
- Directory.Build.props for shared build properties
- Directory.Packages.props for centralized package versioning
- Solution-wide .editorconfig for consistent code style

## Technical Constraints

- Appears to be compatible with .NET Standard and .NET Core
- May have specific language version constraints (referenced in docs/decisions)
