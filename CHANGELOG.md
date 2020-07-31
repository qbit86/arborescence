# Changelog

## [Unreleased] - 2020-07-31
### Added
- Models: `SimpleIncidenceGraph.UndirectedBuilder`.

## [0.3.0] - 2020-07-31
### Added
- Primitives: non-generic `Endpoints` struct.
- Primitives: Target framework `netstandard2.1` to support nullability attributes.
- Primitives: `Empty` property to enumerators.
- Models: non-generic `UndirectedIndexedIncidenceGraphPolicy` struct.
- Models: `MutableIndexedIncidenceGraph`.

### Changed
- Primitives: `ArraySegmentEnumerator<T>` constructor.
- Primitives: Made `Current` properties of enumerator structs `readonly`.
- Primitives: Bumped minimum target framework to `netstandard1.3`.
- Models: Bumped minimum target framework to `netstandard1.3`.
- Models: Reimplement `IndexedIncidenceGraph.Builder` and `UndirectedIndexedIncidenceGraph.Builder` with sorting.
- Models: Reimplement `SimpleIncidenceGraph` with `Endpoints` as edge.

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

[Unreleased]: https://github.com/qbit86/arborescence/compare/models-0.3.0...HEAD
[0.3.0]: https://github.com/qbit86/arborescence/compare/models-0.2.0...models-0.3.0
[0.2.0]: https://github.com/qbit86/arborescence/compare/traversal-0.1.1...models-0.2.0
[0.1.1]: https://github.com/qbit86/arborescence/compare/abstractions-0.1.0...traversal-0.1.1
[0.1.0]: https://github.com/qbit86/arborescence/releases/tag/abstractions-0.1.0
