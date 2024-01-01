# Changelog

## [Unreleased] - 2024-01-01

### Added

- Models: `net8.0` target framework moniker to support `FrozenSet<T>`
- Models:  `FrozenSetEnumeratorProvider<T>`, `HashSetEnumeratorProvider<T>`
- Models: `ArrayEnumeratorProvider<T>`, `ArraySegmentEnumeratorProvider<T>`
- Models: `ReadOnlyAdjacencyGraphFactory<TVertex>`, `ReadOnlyIncidenceGraphFactory<TVertex, TEdge>`

### Changed

- Abstractions: Fixed package description.
- Primitives.Specialized: Fixed `ArrayPrefixEnumerator<T>` constructor.

## [0.16.2] - 2023-07-28

### Added

- Models: `ListEnumeratorProvider<T>`

- Traversal: `Adjacency.EagerBfs<TVertex, TNeighborEnumerator>`, `Adjacency.EagerBfs<TVertex>`
- Traversal: `Adjacency.EagerDfs<TVertex, TNeighborEnumerator>`, `Adjacency.EagerDfs<TVertex>`
- Traversal: `Adjacency.RecursiveDfs<TVertex, TNeighborEnumerator>`, `Adjacency.RecursiveDfs<TVertex>`

- Traversal.Specialized: `Adjacency.EagerBfs<TNeighborEnumerator>`, `Adjacency.EagerBfs`
- Traversal.Specialized: `Adjacency.EagerDfs<TNeighborEnumerator>`, `Adjacency.EagerDfs`
- Traversal.Specialized: `Adjacency.EnumerableBfs<TNeighborEnumerator>`, `Adjacency.EnumerableBfs`
- Traversal.Specialized: `Adjacency.EnumerableDfs<TNeighborEnumerator>`, `Adjacency.EnumerableDfs`
- Traversal.Specialized: `Adjacency.EnumerableGenericSearch<TNeighborEnumerator>`, `Adjacency.EnumerableGenericSearch`
- Traversal.Specialized: `Adjacency.RecursiveDfs<TNeighborEnumerator>`, `Adjacency.RecursiveDfs`
- Traversal.Specialized: `Incidence.EagerBfs<TEdge>`, `Incidence.EagerBfs`
- Traversal.Specialized: `Incidence.EagerDfs<TEdge>`, `Incidence.EagerDfs`
- Traversal.Specialized: `Incidence.EnumerableBfs<TEdge, TEdgeEnumerator>`, `Incidence.EnumerableBfs<TEdge>`
- Traversal.Specialized: `Incidence.EnumerableDfs<TEdge, TEdgeEnumerator>`, `Incidence.EnumerableDfs<TEdge>`
- Traversal.Specialized: `Incidence.EnumerableGenericSearch<TEdge, TEdgeEnumerator>`, `Incidence.EnumerableGenericSearch<TEdge>`
- Traversal.Specialized: `Incidence.RecursiveDfs<TEdge, TEdgeEnumerator>`, `Incidence.RecursiveDfs<TEdge>`

### Changed

- Abstractions: Renamed type parameters and fixed XML-doc comments.

- Models: Moved `IncidenceEnumerator<>` to `Arborescence.Models` namespace.
- Models: Renamed type parameters and fixed XML-doc comments.

- Models.Specialized: Renamed type parameters and fixed XML-doc comments.

- Primitives: Fixed `Endpoints<TVertex>`.

- Primitives.Specialized: Fixed `Int32Dictionary<>`.

- Traversal: Renamed type parameters and fixed XML-doc comments.

## [0.16.1] - 2023-06-11

### Added

- Models.Specialized: XML-doc comments
- Primitives.Specialized: XML-doc comments

### Changed

- Models: Improved package description.
- Primitives.Specialized: Renamed `IndexedColorDictionary` to `Int32ColorDictionary`.

## [0.16] - 2023-06-05

### Added

- Abstractions: `IEnumeratorProvider<>`, `IDictionaryAddition<>`, `IReadOnlyMultimapConcept<>`
- Models: `AdjacencyEnumerator<>`, `IncidenceEnumerator<>`
- Models: `ListAdjacencyGraph<>`, `ListIncidenceGraph<>`, `ReadOnlyAdjacencyGraph<>`, `ReadOnlyIncidenceGraph<>`
- Models: `ArrayEnumeratorProvider<T>`, `ArraySegmentEnumeratorProvider<T>`
- Models: `ListMultimapConcept<>`, `ReadOnlyMultimapConcept<>`
- Models.Specialized: `Int32AdjacencyGraph`, `Int32IncidenceGraph`
- Primitives: `DefaultAbsence<T>`
- Primitives.Specialized: `Int32ReadOnlyDictionary<>`, `Int32IndirectReadOnlyDictionary<>`, `Int32IndirectDictionary<>`

### Changed

- Abstractions: Renamed some interfaces and some methods.
- Models: Moved a lot of types to Models.Specialized project.
- Traversal: Moved `Endpoints<TVertex>` and `Color` types to Primitives project.
- Traversal: Moved `IndexedColorDictionary` types to Primitives.Specialized project.
- Traversal: Moved `EnumerableBfs<>` and `EnumerableDfs<>` types to Traversal.Specialized project.

### Removed

- Dropped support of `.netcoreapp3.1` and several other TFMs.
- Models: Dropped support of `netstandard1.0` TFM.
- Models: `Int32Endpoints`
- Models: `SimpleIncidenceGraph`, `MutableSimpleIncidenceGraph`, `MutableUndirectedSimpleIncidenceGraph`
- Models: `IndexedIncidenceGraph`, `MutableIndexedIncidenceGraph`, `UndirectedIndexedIncidenceGraph`, `MutableUndirectedIndexedIncidenceGraph`

## [0.15.3] - 2023-02-09

### Added

- Traversal: `EagerBfs<TVertex, TEdge, TEdgeEnumerator>`, `EagerDfs<TVertex, TEdge, TEdgeEnumerator>`, `RecursiveDfs<TVertex, TEdge, TEdgeEnumerator>`
- Traversal: Added a cancellation token to the eager versions of the algorithms.

### Changed

- Traversal: Changed the type for the sources from `TSourceEnumerator : IEnumerator<TVertex>` to `TSourceCollection : IEnumerable<TVertex>`.

### Removed

- Traversal: `EagerBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>`, `EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>`, `RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>`

## [0.15.2] - 2023-01-25

### Added

- Traversal: `Incidence.EnumerableGenericSearch<TVertex, TEdge>`, `Incidence.EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>`
- Traversal: `Incidence.EnumerableBfs<TVertex, TEdge>`, `Incidence.EnumerableBfs<TVertex, TEdge, TEdgeEnumerator>`
- Traversal: `Incidence.EnumerableDfs<TVertex, TEdge>`, `Incidence.EnumerableDfs<TVertex, TEdge, TEdgeEnumerator>`

### Changed

- Traversal: Changed return type of the adjacency algorithms from `IEnumerator<T>` to `IEnumerable<T>`.

### Removed

- Traversal: `GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator>`, `EnumerableBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>`, `EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>`

## [0.15.1] - 2023-01-19

### Added

- Traversal: `Adjacency.EnumerableDfs<TVertex, TNeighborEnumerator>`, `Adjacency.EnumerableDfs<TVertex>`

## [0.15.0] - 2023-01-17

### Added

- Abstractions: `IForwardIncidence<TVertex, TEdge, TEdges>`
- Traversal: `Adjacency.EnumerableGenericSearch<TVertex, TNeighborEnumerator>`, `Adjacency.EnumerableGenericSearch<TVertex>`
- Traversal: `Adjacency.EnumerableBfs<TVertex, TNeighborEnumerator>`, `Adjacency.EnumerableBfs<TVertex>`
- Samples: Project Arborescence.FlightGraphs

### Changed

- Abstractions: Renamed `EnumerateAdjacentVertices(TVertex)` to `EnumerateNeighbors(TVertex vertex)`.
- Abstractions: Elaborated Readme file.
- Models: Renamed non-generic type `Endpoints` to `Int32Endpoints`.
- Primitives: Moved `Endpoints<TVertex>` to Traversal project.

### Removed

- Abstractions: `ITraversable<TVertex, TEdge, TEdges>`

## [0.14.1] - 2022-01-10

### Added

- Models: `Arborescence.Models.Compatibility` namespace with graph implementations for legacy TFMs

## [0.14.0] - 2022-01-07

### Added

- Abstractions: `IHeadIncidence<TVertex, TEdge>`, `ITailIncidence<TVertex, TEdge>`, `IOutEdgesIncidence<TVertex, TEdges>`, `ITraversable<TVertex, TEdge, TEdges>`
- Models: `AdditiveMonoid<T>`

### Changed

- Dropped support of `netcoreapp2.1` in favor of `netcoreapp3.1`.
- Traversal: Added `notnull` constraint for `TVertex` where appropriate.

### Removed

- Abstractions: `IIncidenceGraph<TVertex, TEdge, TEdges>`, `IBidirectionalGraph<TVertex, TEdge, TEdges>`
- Models: `Int32AdditiveMonoid`
- Models: types in `Arborescence.Models.Compatibility` namespace
- Primitives: `ArraySegmentEnumerator<T>`

## [0.13.1] - 2021-07-07

### Added

- Models: Missing types for .NET 5 target framework.

### Changed

- Primitives: `IndexedSet` and `CompactSet` now implement `IReadOnlySet<int>` when targeting .NET 5.

## [0.13.0] - 2021-06-27

### Added

- Abstractions: Nullable attributes.
- Models: Nullable attributes.
- Primitives: `IndexedDictionary<TKey, TValue, TIndexMap>` — an indirect key-to-value map via an intermediate index map.
- Primitives: `IndexedDictionary<TKey, TValue, TIndexMap, TDummy>` — an indirect key-to-value map with a marker for missing values.
- Primitives: More nullability annotations.
- Primitives: `readonly` modifiers to the members of non-readonly enumerators.
- Traversal: Nullable attributes.

### Changed

- Primitives: Fixed equality checking for `IndexedDictionary<TValue, TDummy>`.
- Primitives: `IndexedDictionary<TValue, TDummy>.Count` now throws `NotSupportedException`.
- Primitives: Fixed `GetHashCode()` for `Endpoints<TVertex>`.

## [0.12.0] - 2021-06-10

### Added

- Traversal: Calling `Dispose()` for out-edges enumerators.
- Traversal: Moved ``EnumerableBfs`3`` and ``EnumerableDfs`3`` to `Arborescence.Traversal.Specialized` namespace.
- Primitives: `IndexedDictionary<TValue, TDummy>` — a map with a marker for missing values.

### Changed

- Traversal: Made public methods of ``GenericSearch`4``, ``EnumerableBfs`3``, ``EnumerableBfs`4``, ``EnumerableDfs`3``, ``EnumerableDfs`4`` to be non-iterator, so arguments check takes place eagerly.

### Removed

- Traversal: Moved ``EnumerableBfs`3`` and ``EnumerableDfs`3`` from `Arborescence.Traversal` namespace.

## [0.11.0] - 2021-05-30

### Added

- Primitives: Moved `IndexedSet` and `IndexedSet` from Models.
- Primitives: `IndexedDictionary<TValue>`.
- Traversal: `ContainsKey()` and `TryGetValue()` to `IndexedColorDictionary`.

### Removed

- Models: Moved `IndexedSet` and `IndexedSet` to Primitives.

### Fixed

- Traversal: Exception in indexer for `IndexedColorDictionary`.

## [0.10.0] - 2021-05-30

### Added

- Models: `CompactSet` structure implementing `ISet<int>` with bit array as backing store.

### Changed

- Abstractions: Renamed `IMonoidPolicy<T>` to `IMonoid<T>`.
- Models: Renamed `Int32AdditiveMonoidPolicy` to `Int32AdditiveMonoid`.

### Fixed

- Traversal: Added throwing `ArgumentNullException` in case if `colorMap`, or `exploredSet`, or `fringe` are `null`.

### Removed

- Abstractions: `IReadOnlySetPolicy<>`, `ISetPolicy<>`, `IMapPolicy<>`, `IReadOnlyMapPolicy<>`, `IContainerPolicy<>`.
- Models: `IndexedSetPolicy`, `CompactSetPolicy`, `IndexedColorMapPolicy`.
- Traversal: ``EagerBfs`6``, ``EagerDfs`6``, ``RecursiveDfs`6``, ``EnumerableBfs`6``, ``EnumerableDfs`6``, ``GenericSearch`8``.

## [0.9.0] - 2021-01-13

### Added

- Abstractions: Nullability attributes for legacy target frameworks.

### Changed

- Traversal: Renamed `Bfs<>` and `Dfs<>` to `EnumerableBfs<>` and `EnumerableDfs<>` for consistency with existing generic types.

### Removed

- Abstractions: `netcoreapp3.1` from target frameworks.

## [0.8.1] - 2021-01-08

### Added

- Traversal: `EagerBfs<>` and `EagerDfs<>` generic types without concept for the vertex color map.
- Traversal: `IndexedColorDictionary` structure implementing `IDictionary<int, Color>` with an array as backing store.

## [0.8.0] - 2021-01-06

### Added

- Models: `net5.0` as another target framework.
- Models: `IndexedSet` structure implementing `ISet<int>` with an array as backing store.
- Traversal: Another `GenericSearch<>` struct with less type parameters.
- Traversal: `EnumerableBfs<>` and `EnumerableDfs<>` generic types without concept for the vertex set.

### Fixed

- Traversal: XML-doc comments.

## [0.7.1] - 2020-12-26

### Changed

- Marked assemblies as CLS-compliant.
- Abstractions: Added nullability annotations.
- Primitives: Replaced throwing of `IndexOutOfRangeException` with `ArgumentOutOfRangeException`.
- Traversal: Renamed edge and vertex handlers to `EdgeHandler<>` and `VertexHandler<>`.

## [0.7.0] - 2020-12-26

### Added

- Abstractions: `IAdjacency<>`, `IAdjacencyMatrix<>` interfaces.

### Changed

- Dropped support of `netcoreapp2.0` in favor of `netcoreapp2.1`.
- Models: Added arguments check to `IndexedSetPolicy.Add()` to ensure the invariant of “contains after add”.
- Traversal: Added arguments check to `IndexedColorMapPolicy.AddOrUpdate()` to ensure the invariant of “contains after add”.
- Traversal: Refactored `GenericSearch<>` to be O(_n_) instead of O(_m_) in terms of worst-case space complexity.
- Traversal: Renamed `InstantBfs<>` and `InstantDfs<>` to `EagerBfs<>` and `EagerDfs<>` respectively.

### Removed

- Abstractions: `IHeadConcept<>`, `ITailConcept<>`, `IOutEdgesConcept<>`, `IInEdgesConcept<>`.
- Traversal: `ReverseDfs<>` since its worst-case space complexity is O(_m_).

## [0.6.0] - 2020-11-10

### Added

- Abstractions: `IReadOnlySetPolicy<>` and `IReadOnlyMapPolicy<>` interfaces.

### Removed

- Abstractions: Graph concepts.
- Models: Graph concept implementations.

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

- Models: Made `initialVertexCount` optional.

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
- Models: Some obsolete concepts.

## [0.1.1] - 2020-07-13

### Added

- Generating XML documentation files.
- Primitives: Basic blocks for building algorithms and data structures.
- Models: Data structures for graphs and concept models to manipulate them.
- Traversal: Graph traversal algorithms.

## [0.1.0] - 2020-07-05

### Added

- Abstractions: The interface for graphs to be examined in a data-structure agnostic fashion.

[Unreleased]: https://github.com/qbit86/arborescence/compare/arborescence-0.16.2...HEAD

[0.16.2]: https://github.com/qbit86/arborescence/compare/arborescence-0.16.1...arborescence-0.16.2

[0.16.1]: https://github.com/qbit86/arborescence/compare/models.specialized-0.16-preview...arborescence-0.16.1

[0.16]: https://github.com/qbit86/arborescence/compare/traversal-0.15.3...models.specialized-0.16-preview

[0.15.3]: https://github.com/qbit86/arborescence/compare/traversal-0.15.2...traversal-0.15.3

[0.15.2]: https://github.com/qbit86/arborescence/compare/traversal-0.15.1...traversal-0.15.2

[0.15.1]: https://github.com/qbit86/arborescence/compare/arborescence-0.15.0...traversal-0.15.1

[0.15.0]: https://github.com/qbit86/arborescence/compare/models-0.14.1...arborescence-0.15.0

[0.14.1]: https://github.com/qbit86/arborescence/compare/arborescence-0.14.0...models-0.14.1

[0.14.0]: https://github.com/qbit86/arborescence/compare/arborescence-0.13.1...arborescence-0.14.0

[0.13.1]: https://github.com/qbit86/arborescence/compare/arborescence-0.13.0...arborescence-0.13.1

[0.13.0]: https://github.com/qbit86/arborescence/compare/arborescence-0.12.0...arborescence-0.13.0

[0.12.0]: https://github.com/qbit86/arborescence/compare/arborescence-0.11.0...arborescence-0.12.0

[0.11.0]: https://github.com/qbit86/arborescence/compare/arborescence-0.10.0...arborescence-0.11.0

[0.10.0]: https://github.com/qbit86/arborescence/compare/arborescence-0.9.0...arborescence-0.10.0

[0.9.0]: https://github.com/qbit86/arborescence/compare/traversal-0.8.1...arborescence-0.9.0

[0.8.1]: https://github.com/qbit86/arborescence/compare/arborescence-0.8.0...traversal-0.8.1

[0.8.0]: https://github.com/qbit86/arborescence/compare/arborescence-0.7.1...arborescence-0.8.0

[0.7.1]: https://github.com/qbit86/arborescence/compare/arborescence-0.7.0...arborescence-0.7.1

[0.7.0]: https://github.com/qbit86/arborescence/compare/abstractions-0.6.0...arborescence-0.7.0

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
