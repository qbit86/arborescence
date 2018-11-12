namespace Ubiquitous
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    // https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/System/ThrowHelper.cs
    internal static class ThrowHelper
    {
        internal static void ThrowArgumentException_DestinationTooShort()
        {
            throw new ArgumentException(SR.Argument_DestinationTooShort);
        }

        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        internal static void ThrowArgumentOutOfRange_IndexException()
        {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
        }

        internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array array, int offset, int count)
        {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
        }

        internal static void ThrowInvalidOperationException(ExceptionResource resource)
        {
            throw new InvalidOperationException(GetResourceString(resource));
        }

        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumEnded);
        }

        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumNotStarted);
        }

        internal static void ThrowNotSupportedException()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception GetArraySegmentCtorValidationFailedException(Array array, int offset, int count)
        {
            if (array == null)
                return new ArgumentNullException(nameof(array));

            if (offset < 0)
                return new ArgumentOutOfRangeException(nameof(offset), SR.ArgumentOutOfRange_NeedNonNegNum);

            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), SR.ArgumentOutOfRange_NeedNonNegNum);

            Debug.Assert(array.Length - offset < count);

            return new ArgumentException(SR.Argument_InvalidOffLen);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument,
            ExceptionResource resource)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetArgumentName(ExceptionArgument argument)
        {
            switch (argument)
            {
                case ExceptionArgument.array:
                    return nameof(ExceptionArgument.array);
                case ExceptionArgument.count:
                    return nameof(ExceptionArgument.count);
                case ExceptionArgument.index:
                    return nameof(ExceptionArgument.index);
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionArgument Enum.");
                    return String.Empty;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ExceptionResource resource)
        {
            switch (resource)
            {
                case ExceptionResource.ArgumentOutOfRange_Index:
                    return SR.ArgumentOutOfRange_Index;
                case ExceptionResource.InvalidOperation_NullArray:
                    return SR.InvalidOperation_NullArray;
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
