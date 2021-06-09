namespace Arborescence.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class AStarHelper
    {
        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_NegativeWeight()
        {
            throw new InvalidOperationException("The graph may not contain an edge with negative weight.");
        }
    }
}
