# Traversal — Arborescence Graph Library

[![Arborescence.Traversal version](https://img.shields.io/nuget/v/Arborescence.Traversal.svg?label=Traversal&logo=nuget)](https://nuget.org/packages/Arborescence.Traversal/)

## Basic usage — Adjacency

```cs
public sealed class MyAdjacencyNetwork :
    IAdjacency<MyNode, IEnumerator<MyNode>>
{
    public IEnumerator<MyNode> EnumerateNeighbors(
        MyNode vertex) => ...
}
```
