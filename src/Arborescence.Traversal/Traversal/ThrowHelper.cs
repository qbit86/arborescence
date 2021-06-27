namespace Arborescence.Traversal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowArgumentNullException(string argument) => throw new ArgumentNullException(argument);

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() => throw new NotSupportedException();
    }
}
