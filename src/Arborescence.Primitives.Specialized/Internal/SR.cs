namespace Arborescence.Primitives
{
    // https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/Resources/Strings.resx
    internal static class SR
    {
        internal const string Argument_DestinationTooShort = "Destination is too short.";

        internal const string Argument_InvalidOffLen =
            "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.";

        internal const string InvalidOperation_EnumEnded = "Enumeration already finished.";
        internal const string InvalidOperation_EnumNotStarted = "Enumeration has not started. Call MoveNext.";
        internal const string InvalidOperation_NullArray = "The underlying array is null.";
        internal const string UnreachableLocation = "This program location is thought to be unreachable.";
    }
}
