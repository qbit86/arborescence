#if !NETSTANDARD2_1 && !NETCOREAPP3_0
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that when a method returns <see cref="ReturnValue"/>,
    /// the parameter may be null even if the corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the specified return value condition.</summary>
        /// <param name="returnValue">
        /// The return value condition. If the method returns this value, the associated parameter may be null.
        /// </param>
        internal MaybeNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }

        /// <summary>Gets the return value condition.</summary>
        internal bool ReturnValue { get; }
    }
}
#endif
