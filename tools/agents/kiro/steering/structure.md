# Project Structure

## Solution Organization

The Arborescence solution follows a clear modular structure with separation by purpose:

```
Arborescence/
├── assets/                 # Icons, diagrams, signing keys
├── benchmarks/             # Performance benchmarks
├── docs/                   # Documentation
├── samples/                # Example applications
├── src/                    # Main library code
├── tests/                  # Unit tests
└── tools/                  # Development utilities
```

## Source Code Structure (`src/`)

### Core Libraries

- **Arborescence.Abstractions**: Interfaces and core concepts
- **Arborescence.Primitives**: Basic data structures and building blocks
- **Arborescence.Models**: Generic graph data structures
- **Arborescence.Traversal**: Graph traversal algorithms (BFS, DFS)
- **Arborescence.Search**: Search algorithms (Dijkstra, A*)

### Specialized Libraries

- **Arborescence.Primitives.Specialized**: Optimized primitives for integer vertices
- **Arborescence.Models.Specialized**: Specialized graph structures for performance
- **Arborescence.Traversal.Specialized**: Optimized traversal for integer vertices

### Shared Code

- **src/Shared/**: Contains shared projects (`.shproj` files) for common utilities
    - NullableAttributes, TryHelpers, ValueStack, ValueQueue, etc.

## Test Structure (`tests/`)

- **Tests.Models**: Tests for generic graph models
- **Tests.Models.Specialized**: Tests for specialized implementations
- **Tests.Primitives.Specialized**: Tests for specialized primitives
- **Tests.Traversal**: Tests for traversal algorithms
- **tests/Shared/**: Shared test utilities

## Sample Applications (`samples/`)

- **Samples.Abstractions.FlightGraphs**: Flight network examples
- **Samples.Models.ListGraphs**: List-based graph examples
- **Samples.Traversal.BasicUsage**: Basic traversal examples
- **Samples.Traversal.EagerBfsDemo**: Eager BFS demonstrations
- **Samples.Traversal.EagerDfsDemo**: Eager DFS demonstrations
- **Samples.Traversal.EnumerableDfsDemo**: Enumerable DFS examples
- **Samples.Traversal.Specialized.BasicUsage**: Specialized algorithm examples

## Build Configuration

- **Directory.Build.props**: Root-level build properties
- **Directory.Packages.props**: Centralized package version management
- **global.json**: .NET SDK version specification
- **nuget.config**: NuGet package source configuration

## Naming Conventions

- **Namespaces**: Follow `Arborescence.[Component]` pattern
- **Projects**: Use full namespace as project name
- **Specialized**: Append `.Specialized` for optimized implementations
- **Benchmarks**: Prefix with `Benchmarks.` followed by component name
- **Samples**: Prefix with `Samples.` followed by component and purpose
- **Tests**: Prefix with `Tests.` followed by component name
