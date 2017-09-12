namespace Ubiquitous
{
    using System.Collections.Generic;

    internal interface ITraversal<TVertex, TEdge, TColorMap>
    {
        IEnumerable<Step<DfsStepKind, TVertex, TEdge>> Traverse(TVertex vertex, TColorMap colorMap);
    }
}
