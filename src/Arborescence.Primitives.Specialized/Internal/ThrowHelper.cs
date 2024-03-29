﻿namespace Arborescence.Primitives
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
        internal static void ThrowAddingDuplicateWithKeyArgumentException(int key) =>
            throw new ArgumentException($"An item with the same key has already been added. Key: {key}", nameof(key));

        [DoesNotReturn]
        internal static void ThrowDestinationArrayTooSmallException() => throw new ArgumentException(
            "Destination array is not long enough to copy all the items in the collection. Check array index and length.");

        [DoesNotReturn]
        internal static TResult ThrowArgumentNullException<TResult>(string paramName) =>
            throw new ArgumentNullException(paramName);

        [DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument) =>
            throw new ArgumentNullException(GetArgumentName(argument));

        [DoesNotReturn]
        internal static TResult ThrowKeyNotFoundException<TResult>(int key) =>
            throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");

        [DoesNotReturn]
        internal static void ThrowKeyNotFoundException() =>
            throw new KeyNotFoundException("The given key was not present in the dictionary.");

        [DoesNotReturn]
        internal static TResult ThrowKeyNotFoundException<TResult>() =>
            throw new KeyNotFoundException("The given key was not present in the dictionary.");

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
                case ExceptionArgument.items:
                    return nameof(ExceptionArgument.items);
                default:
                    Debug.Assert(false, "The enum value is not defined, please check the ExceptionArgument enum.");
                    return string.Empty;
            }
        }
    }

    internal enum ExceptionArgument
    {
        array,
        items
    }
}
