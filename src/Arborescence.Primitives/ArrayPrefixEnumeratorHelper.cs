namespace Arborescence
{
    using System;
#if NETSTANDARD2_1
    using System.Diagnostics.CodeAnalysis;

#endif

    internal static class ArrayPrefixEnumeratorHelper
    {
#if NETSTANDARD2_1
        [DoesNotReturn]
#endif
        internal static void ThrowCtorValidationFailedExceptions(Array array, int count)
        {
            throw GetCtorValidationFailedException(array, count);
        }

#if NETSTANDARD2_1
        [DoesNotReturn]
#endif
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumNotStarted);
        }

#if NETSTANDARD2_1
        [DoesNotReturn]
#endif
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumEnded);
        }

        private static Exception GetCtorValidationFailedException(Array array, int count)
        {
            if (array is null)
                return new ArgumentNullException(nameof(array));

            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), SR.ArgumentOutOfRange_NeedNonNegNum);

            if (count > array.Length)
                return new ArgumentOutOfRangeException(nameof(count));

            throw new InvalidOperationException(SR.UnreachableLocation);
        }
    }
}
