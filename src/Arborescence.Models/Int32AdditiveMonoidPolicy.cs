namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct Int32AdditiveMonoidPolicy : IMonoidPolicy<int>
    {
        public int Identity => 0;

        public int Combine(int left, int right) => left + right;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
