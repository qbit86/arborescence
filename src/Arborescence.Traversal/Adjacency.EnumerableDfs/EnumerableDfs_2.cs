namespace Arborescence.Traversal.Adjacency
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph — implemented as enumerator.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static partial class EnumerableDfs<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        internal readonly record struct StackFrame(TVertex Current, TNeighborEnumerator NeighborEnumerator) :
            IDisposable
        {
            public void Dispose() => NeighborEnumerator.Dispose();
        }
    }
}
