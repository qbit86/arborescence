namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        public EnumerableDfs(TGraphPolicy graphPolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            GraphPolicy = graphPolicy;
            ExploredSetPolicy = exploredSetPolicy;
        }

        private readonly struct StackFrame
        {
            private readonly TVertex _exploredVertex;
            private readonly TEdge _inEdge;
            private readonly bool _hasInEdge;

            internal StackFrame(TVertex exploredVertex)
            {
                _exploredVertex = exploredVertex;
                _inEdge = default;
                _hasInEdge = false;
            }

            internal StackFrame(TVertex exploredVertex, TEdge inEdge)
            {
                _exploredVertex = exploredVertex;
                _inEdge = inEdge;
                _hasInEdge = true;
            }

            internal TVertex ExploredVertex => _exploredVertex;

            internal bool TryGetInEdge(out TEdge inEdge)
            {
                inEdge = _inEdge;
                return _hasInEdge;
            }
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct LegacyDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

        public LegacyDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
