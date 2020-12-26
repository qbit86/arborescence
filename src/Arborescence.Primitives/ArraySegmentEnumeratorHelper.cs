namespace Arborescence
{
    using System;
    using System.Runtime.CompilerServices;

#if NETSTANDARD2_1 || NETCOREAPP3_1
    using System.Diagnostics.CodeAnalysis;

#endif

    internal static class ArraySegmentEnumeratorHelper
    {
#if NETSTANDARD2_1 || NETCOREAPP3_1
        [DoesNotReturn]
#endif
        internal static void ThrowCtorValidationFailedExceptions(Array array, int start, int endExclusive)
        {
            throw GetCtorValidationFailedException(array, start, endExclusive);
        }

#if NETSTANDARD2_1 || NETCOREAPP3_1
        [DoesNotReturn]
#endif
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumNotStarted);
        }

#if NETSTANDARD2_1 || NETCOREAPP3_1
        [DoesNotReturn]
#endif
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded()
        {
            throw new InvalidOperationException(SR.InvalidOperation_EnumEnded);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception GetCtorValidationFailedException(Array array, int start, int endExclusive)
        {
            if (array is null)
                return new ArgumentNullException(nameof(array));

            if (start < 0)
                return new ArgumentOutOfRangeException(nameof(start), SR.ArgumentOutOfRange_NeedNonNegNum);

            if (start > array.Length)
                return new ArgumentOutOfRangeException(nameof(start));

            if (endExclusive < 0)
                return new ArgumentOutOfRangeException(nameof(endExclusive), SR.ArgumentOutOfRange_NeedNonNegNum);

            if (endExclusive > array.Length)
                return new ArgumentOutOfRangeException(nameof(endExclusive));

            throw new InvalidOperationException(SR.UnreachableLocation);
        }
    }
}
