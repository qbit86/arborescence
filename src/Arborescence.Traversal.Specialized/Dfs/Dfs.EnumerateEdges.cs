namespace Arborescence.Traversal.Dfs
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct Dfs<TGraph, TEdge, TEdgeEnumerator>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, int vertexCount, int source)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if ((uint)source >= (uint)vertexCount)
                throw new ArgumentOutOfRangeException(nameof(source));

            throw new NotImplementedException();
        }
    }
}
