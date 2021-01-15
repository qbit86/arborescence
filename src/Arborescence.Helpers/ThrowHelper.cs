namespace Arborescence
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/ThrowHelper.cs

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowArgumentException_DestinationTooShort()
        {
            throw new ArgumentException(SR.Argument_DestinationTooShort);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        [DoesNotReturn]
        internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array array, int offset, int count)
        {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException(ExceptionResource resource)
        {
            throw new InvalidOperationException(GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumEnded);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumNotStarted);
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException()
        {
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception GetArraySegmentCtorValidationFailedException(Array array, int offset, int count)
        {
            if (array is null)
                return new ArgumentNullException(nameof(array));

            if (offset < 0)
                return new ArgumentOutOfRangeException(nameof(offset), SR.ArgumentOutOfRange_NeedNonNegNum);

            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), SR.ArgumentOutOfRange_NeedNonNegNum);

            Debug.Assert(array.Length - offset < count);

            return new ArgumentException(SR.Argument_InvalidOffLen);
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
                case ExceptionArgument.start:
                    return nameof(ExceptionArgument.start);
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionArgument enum.");
                    return string.Empty;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ExceptionResource resource)
        {
            switch (resource)
            {
                case ExceptionResource.InvalidOperation_NullArray:
                    return SR.InvalidOperation_NullArray;
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionResource enum.");
                    return string.Empty;
            }
        }
    }

    internal enum ExceptionResource
    {
        InvalidOperation_NullArray
    }
}
