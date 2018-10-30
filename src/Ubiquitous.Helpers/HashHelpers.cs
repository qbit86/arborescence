namespace Ubiquitous
{
    // https://github.com/dotnet/corert/blob/master/src/Common/src/System/Numerics/Hashing/HashHelpers.cs
    internal static class HashHelpers
    {
        internal static int Combine(int h1, int h2)
        {
            // RyuJIT optimizes this to use the ROL instruction
            // Related GitHub pull request: dotnet/coreclr#1830
            uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
            return ((int)rol5 + h1) ^ h2;
        }
    }
}
