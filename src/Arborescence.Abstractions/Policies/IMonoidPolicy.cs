namespace Arborescence
{
    /// <summary>
    /// Represents a monoid — an algebraic structure with a single associative binary operation and an identity element.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the monoid.</typeparam>
    public interface IMonoidPolicy<T>
    {
        /// <summary>
        /// Gets the identity element of the monoid.
        /// </summary>
        T Identity { get; }

        /// <summary>
        /// Combines to elements of the monoid into one element of the same monoid.
        /// </summary>
        /// <param name="left">The left element to combine.</param>
        /// <param name="right">The right element to combine.</param>
        /// <returns>The result of combining the elements.</returns>
        T Combine(T left, T right);
    }
}
