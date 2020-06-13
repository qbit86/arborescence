namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using Collections;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        private IEnumerator<TEdge> EnumerateEdgesCore<TContainer>(
            TGraph graph, TContainer queue, TExploredSet exploredSet)
            where TContainer : IContainer<TVertex>
        {
            throw new NotImplementedException();
        }
    }
    // ReSharper restore UnusedTypeParameter
}
