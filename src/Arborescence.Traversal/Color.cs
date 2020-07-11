namespace Arborescence.Traversal
{
#pragma warning disable CA1028 // Enum Storage should be Int32
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Specifies labels used to mark vertices while traversing a graph.
    /// </summary>
    public enum Color : byte
    {
        None = 0,
        White,
        Gray,
        Black
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CA1028 // Enum Storage should be Int32
}
