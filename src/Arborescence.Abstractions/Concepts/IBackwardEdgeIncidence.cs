namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    public interface IBackwardEdgeIncidence<TVertex, in TEdge>
    {
        bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail);
    }
}
