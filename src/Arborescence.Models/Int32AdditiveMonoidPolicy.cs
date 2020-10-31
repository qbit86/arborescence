namespace Arborescence.Models
{
    /// <summary>
    /// Represents a monoid for the elements of the type <see cref="int"/> with 0 as the identity element
    /// and addition as the binary operation.
    /// </summary>
    public readonly struct Int32AdditiveMonoidPolicy : IMonoidPolicy<int>
    {
        /// <inheritdoc/>
        public int Identity => 0;

        /// <inheritdoc/>
        public int Combine(int left, int right) => left + right;
    }
}
