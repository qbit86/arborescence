namespace Ubiquitous
{
    using System;

    // https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/System/ThrowHelper.cs
    // https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/Resources/Strings.resx
    internal static class ThrowHelper
    {
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
        {
            throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
        }

        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
        {
            throw new InvalidOperationException("Enumeration already finished.");
        }
    }
}
