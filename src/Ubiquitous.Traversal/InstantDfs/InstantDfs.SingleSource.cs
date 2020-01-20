namespace Ubiquitous.Traversal
{
    using System;
    using System.Diagnostics;

    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler)
        {
            TraverseCore(graph, startVertex, colorMap, handler, False);
        }

        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
        {
            if (terminationCondition == null)
                throw new ArgumentNullException(nameof(terminationCondition));

            TraverseCore(graph, startVertex, colorMap, handler, terminationCondition);
        }

        private void TraverseCore<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
        {
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            throw new NotImplementedException();
        }

        private static bool False(TGraph graph, TVertex vertex)
        {
            return false;
        }
    }
}
