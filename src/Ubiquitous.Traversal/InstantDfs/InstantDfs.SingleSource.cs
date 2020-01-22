namespace Ubiquitous.Traversal
{
    using System;
    using System.Diagnostics;

    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        private static readonly Func<TGraph, TVertex, bool> s_false = (g, v) => false;

        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler)
            where THandler : IDfsHandler<TGraph, TVertex>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);
        }

        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (terminationCondition == null)
                throw new ArgumentNullException(nameof(terminationCondition));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, terminationCondition);
        }

        private void TraverseCore<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex>
        {
            Debug.Assert(handler != null, "handler != null");
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            ColorMapPolicy.AddOrUpdate(colorMap, startVertex, Color.Gray);
            handler.DiscoverVertex(graph, startVertex);
            throw new NotImplementedException();
        }
    }
}
