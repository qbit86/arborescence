namespace Ubiquitous
{
    using System.Collections.Generic;

    internal interface ITraversal<TStepKind, TVertex, TEdge, TColorMap>
    {
        IEnumerator<Step<TStepKind, TVertex, TEdge>> CreateEnumerator(TVertex vertex, TColorMap colorMap);
    }
}
