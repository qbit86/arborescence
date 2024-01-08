namespace Arborescence.Search
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowVertexNotFoundException(string? vertex) =>
            throw new InvalidOperationException($"The given vertex '{vertex}' was not present in the distance map.");

    }
}
