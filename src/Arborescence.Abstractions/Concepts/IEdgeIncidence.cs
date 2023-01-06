namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    public interface IEdgeIncidence<TVertex, in TEdge>
    {
        bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head);
    }
}
