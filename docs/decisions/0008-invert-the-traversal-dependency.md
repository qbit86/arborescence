---
status: accepted
date: 2026-08-23
---
# Invert the dependency between Traversal and Traversal.Specialized

## Context and Problem Statement

Arborescence.Traversal.Specialized reused the algorithms of Arborescence.Traversal through `InternalsVisibleTo`.
It called internal members such as `Adjacency.EagerBfs<TVertex, TNeighborEnumerator>.TraverseUnchecked()`, and it also took the internal throw helpers and nullable-attribute polyfills of Traversal.
A specialized package therefore shipped the whole generic package as a dependency, and the two packages could never be versioned apart.

How do we share one implementation between the two packages without a dependency between them?

## Decision Drivers

- One copy of each algorithm
- No dependency from Traversal.Specialized to Traversal
- No new public API surface
- Source compatibility for callers

## Considered Options

- Extract the implementation into a shared project (_.shproj_ and _.projitems_)
- Extract the implementation into a non-packable assembly, Arborescence.Traversal.Internal
- Publish Arborescence.Traversal.Internal as a package

## Decision Outcome

Chosen option: "Extract the implementation into a non-packable assembly, Arborescence.Traversal.Internal".

The internal generic types live in the namespaces `Arborescence.Traversal.Internal.Adjacency` and `Arborescence.Traversal.Internal.Incidence`.
Traversal and Traversal.Specialized reference the project with `PrivateAssets="All"` and keep only argument validation of their own:

```xml
<ProjectReference Include="..\Internal\Arborescence.Traversal.Internal\Arborescence.Traversal.Internal.csproj" PrivateAssets="All" />
```

Both packages embed the assembly into every _lib_ folder through `TargetsForTfmSpecificBuildOutput`.
Deterministic builds make the embedded copies byte-identical, so a caller of both packages gets one assembly.

`IBfsHandler<TVertex, TEdge, TGraph>` and `IDfsHandler<TVertex, TEdge, TGraph>` moved to Arborescence.Abstractions, because the public API of Traversal.Specialized constrains on them.
They keep the `Arborescence.Traversal` namespace, and Traversal declares a type forwarder for each one.

### Consequences

- Good, because Traversal.Specialized drops a package dependency.
- Good, because each algorithm has one implementation.
- Good, because the public API stays the same.
- Bad, because two packages ship the same assembly, which needs deterministic builds to stay byte-identical.
- Bad, because the two packages have to declare the dependencies of the embedded assembly, such as System.Buffers on _net462_ and _netstandard2.0_.

## Pros and Cons of the Options

### Shared project

- Good, because it needs no assembly at all.
- Bad, because both assemblies get a private copy of every algorithm, which doubles the code size.
- Bad, because the handler interfaces still need a common assembly.

### Non-packable assembly

- Good, because the implementation is compiled once.
- Good, because `IsPackable` is `false` and `InternalsVisibleTo` keeps the types out of the public API.
- Neutral, because the packages have to embed the assembly by hand.

### Published package

- Good, because NuGet resolves the assembly.
- Bad, because implementation details become public API under semantic versioning.
