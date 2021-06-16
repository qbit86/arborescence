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
        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        [DoesNotReturn]
        internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array array, int offset, int count)
        {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
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
                case ExceptionArgument.items:
                    return nameof(ExceptionArgument.items);
                case ExceptionArgument.start:
                    return nameof(ExceptionArgument.start);
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionArgument enum.");
                    return string.Empty;
            }
        }
    }
}
