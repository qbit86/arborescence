namespace Arborescence.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/ThrowHelper.cs

    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowArgumentNullException(string argument) => throw new ArgumentNullException(argument);

        [DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument) =>
            throw new ArgumentNullException(GetArgumentName(argument));

        [DoesNotReturn]
        internal static void ThrowKeyNotFoundException(int key) =>
            throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() => throw new NotSupportedException();

        [DoesNotReturn]
        internal static TResult ThrowNotSupportedException<TResult>() => throw new NotSupportedException();

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

    internal enum ExceptionArgument
    {
        array,
        count,
        dummy,
        index,
        items,
        start
    }
}
