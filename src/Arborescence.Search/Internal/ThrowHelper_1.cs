namespace Arborescence.Search
{
    using System.Diagnostics.CodeAnalysis;

    internal static class ThrowHelper<T>
    {
        [DoesNotReturn]
        internal static void ThrowVertexNotFoundException(T vertex) =>
            ThrowHelper.ThrowVertexNotFoundException(vertex?.ToString());
    }
}
