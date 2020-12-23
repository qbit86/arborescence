# Changelog

## [Unreleased] - 2020-12-23
### Changed
- Models: Added arguments check to `IndexedSetPolicy.Add()` to ensure the invariant of “contains after add”.
- Traversal: Added arguments check to `IndexedColorMapPolicy.AddOrUpdate()` to ensure the invariant of “contains after add”.
- Dropped support of `netcoreapp2.0` in favor of `netcoreapp2.1`.

## [0.6.0] - 2020-11-10
### Added
- Abstractions: `IReadOnlySetPolicy<>` and `IReadOnlyMapPolicy<>` interfaces.

### Removed
- Abstractions: Graph policy interfaces.
- Models: Graph policy implementations.

## [0.5.0] - 2020-11-03
### Added
- Models: Throwing `Add()` method to builders in addition to non-throwing `TryAdd()`.

### Changed
- Traversal: The order of parameters in `Enumerate*()` signatures for `Bfs<>` and `Dfs<>`.

### Removed
- Models: `GraphBuilderExtensions` class with `TryAdd<>()` extension method.

## [0.4.2] - 2020-10-21
### Added
- Models: Fixed documentation comments.

### Changed
- Models: Make `initialVertexCount` optional. 

## [0.4.1] - 2020-10-15
### Added
- Models: `GraphBuilderExtensions` class with `TryAdd<>()` extension method.

## [0.4.0] - 2020-09-24
### Added
- Models: `netcoreapp2.0` as another target framework.
- Models: `Arborescence.Models.Compatibility` namespace for the legacy types depending on `ArraySegmentEnumerator<T>`.

### Changed
- Models: Reimplemented the graph models in terms of `ArraySegment<T>.Enumerator`, for newer versions of the BCL only.
- Models: Moved original versions of the models to `Arborescence.Models.Compatibility` namespace.
- Primitives: Marked `ArraySegmentEnumerator<T>` as obsolete for newer versions of the BCL.

## [0.3.2] - 2020-09-06
### Added
- Models: Moved `IndexedSetPolicy` and `CompactSetPolicy` from Traversal.
- Traversal: The most basic versions of BFS and DFS.

### Removed
- Traversal: Moved `IndexedSetPolicy` and `CompactSetPolicy` to Models.

## [0.3.1] - 2020-08-13
### Added
- Models: `SimpleIncidenceGraph.UndirectedBuilder`.
- Models: `MutableUndirectedSimpleIncidenceGraph`.

### Fixed
- Models: Argument checking in a couple of graph models.

## [0.3.0] - 2020-07-31
### Added
- Primitives: Non-generic `Endpoints` struct.
- Primitives: Target framework `netstandard2.1` to support nullability attributes.
- Primitives: `Empty` property to enumerators.
- Models: non-generic `UndirectedIndexedIncidenceGraphPolicy` struct.
- Models: `MutableIndexedIncidenceGraph`.

### Changed
- Primitives: `ArraySegmentEnumerator<T>` constructor.
- Primitives: Made `Current` properties of enumerator structs `readonly`.
- Primitives: Bumped minimum target framework to `netstandard1.3`.
- Models: Bumped minimum target framework to `netstandard1.3`.
- Models: Reimplemented `IndexedIncidenceGraph.Builder` and `UndirectedIndexedIncidenceGraph.Builder` with sorting.
- Models: Reimplemented `SimpleIncidenceGraph` with `Endpoints` as edge.

## [0.2.0] - 2020-07-20
### Added
- Models: `SimpleIncidenceGraph`, `SimpleIncidenceGraphPolicy`.

### Changed
- Models: Renamed `AdjacencyListIncidenceGraph` to `IndexedIncidenceGraph`.
- Models: Replaced `AdjacencyListIncidenceGraphBuilder` with `IndexedIncidenceGraph.Builder`.

### Removed
- Models: `EdgeListIncidenceGraph`, `SortedAdjacencyListIncidenceGraph`.
- Models: Some obsolete policies.

## [0.1.1] - 2020-07-13
### Added
- Generating XML documentation files.
- Primitives: Basic blocks for building algorithms and data structures. 
- Models: Data structures for graphs and policy models to manipulate them.
- Traversal: Graph traversal algorithms.

## [0.1.0] - 2020-07-05
### Added
- Abstractions: The interface for graphs to be examined in a data-structure agnostic fashion.

[Unreleased]: https://github.com/qbit86/arborescence/compare/abstractions-0.6.0...HEAD
[0.6.0]: https://github.com/qbit86/arborescence/compare/traversal-0.5.0...abstractions-0.6.0
[0.5.0]: https://github.com/qbit86/arborescence/compare/models-0.4.2...traversal-0.5.0
[0.4.2]: https://github.com/qbit86/arborescence/compare/models-0.4.1...models-0.4.2
[0.4.1]: https://github.com/qbit86/arborescence/compare/models-0.4.0...models-0.4.1
[0.4.0]: https://github.com/qbit86/arborescence/compare/traversal-0.3.2...models-0.4.0
[0.3.2]: https://github.com/qbit86/arborescence/compare/models-0.3.1...traversal-0.3.2
[0.3.1]: https://github.com/qbit86/arborescence/compare/models-0.3.0...models-0.3.1
[0.3.0]: https://github.com/qbit86/arborescence/compare/models-0.2.0...models-0.3.0
[0.2.0]: https://github.com/qbit86/arborescence/compare/traversal-0.1.1...models-0.2.0
[0.1.1]: https://github.com/qbit86/arborescence/compare/abstractions-0.1.0...traversal-0.1.1
[0.1.0]: https://github.com/qbit86/arborescence/releases/tag/abstractions-0.1.0
