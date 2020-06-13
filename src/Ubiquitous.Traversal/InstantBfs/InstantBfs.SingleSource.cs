namespace Ubiquitous.Traversal
{
    using System;

    public readonly partial struct InstantBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
    {
        public void Traverse<THandler>(
            TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            throw new NotImplementedException();
        }
    }
}
