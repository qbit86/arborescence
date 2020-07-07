namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct Int32AdditiveMonoidPolicy : IMonoidPolicy<int>
    {
        /// <inheritdoc/>
        public int Identity => 0;

        /// <inheritdoc/>
        public int Combine(int left, int right) => left + right;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
