# Changelog

## [Unreleased]
### Added
- Models: non-generic `UndirectedIndexedIncidenceGraphPolicy`.

### Changed
- Models: Bumped minimum target platform to `netstandard1.3`.

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

[Unreleased]: https://github.com/qbit86/arborescence/compare/models-0.2.0...HEAD
[0.2.0]: https://github.com/qbit86/arborescence/compare/traversal-0.1.1...models-0.2.0
[0.1.1]: https://github.com/qbit86/arborescence/compare/abstractions-0.1.0...traversal-0.1.1
[0.1.0]: https://github.com/qbit86/arborescence/releases/tag/abstractions-0.1.0
