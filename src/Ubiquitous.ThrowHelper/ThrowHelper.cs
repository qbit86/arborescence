namespace Ubiquitous
{
    using System;
    using System.Diagnostics;

    // https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/System/ThrowHelper.cs
    // https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/Resources/Strings.resx
    internal static class ThrowHelper
    {
        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        public static void ThrowArgumentOutOfRange_IndexException()
        {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
        }

        internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array array, int offset, int count)
        {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
        }

        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
        {
            throw new InvalidOperationException("Enumeration already finished.");
        }

        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
        {
            throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
        }

        private static Exception GetArraySegmentCtorValidationFailedException(Array array, int offset, int count)
        {
            if (array == null)
                return new ArgumentNullException(nameof(array));

            if (offset < 0)
                return new ArgumentOutOfRangeException(nameof(offset), "Non-negative number required.");

            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), "Non-negative number required.");

            Debug.Assert(array.Length - offset < count);

            return new ArgumentException(
                "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        private static string GetArgumentName(ExceptionArgument argument)
        {
            switch (argument)
            {
                case ExceptionArgument.array:
                    return "array";
                case ExceptionArgument.count:
                    return "count";
                case ExceptionArgument.index:
                    return "index";
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionArgument Enum.");
                    return "";
            }
        }

        private static string GetResourceString(ExceptionResource resource)
        {
            switch (resource)
            {
                case ExceptionResource.ArgumentOutOfRange_Index:
                    return "Index was out of range. Must be non-negative and less than the size of the collection.";
                case ExceptionResource.InvalidOperation_NullArray:
                    return "The underlying array is null.";
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionResource Enum.");
                    return "";
            }
        }
    }

    // ReSharper disable InconsistentNaming
    internal enum ExceptionArgument
    {
        array,
        count,
        index
    }

    internal enum ExceptionResource
    {
        ArgumentOutOfRange_Index,
        InvalidOperation_NullArray
    }
    // ReSharper restore InconsistentNaming
}
