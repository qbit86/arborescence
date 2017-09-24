namespace Ubiquitous
{
    using System.Collections.Generic;

    internal interface ITraversal<TVertex, TEdge, TColorMap>
    {
        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> CreateEnumerator(TVertex vertex, TColorMap colorMap);
    }
}
