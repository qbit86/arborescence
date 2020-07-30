namespace Arborescence
{
    using System.Text;

    internal static class EndpointsHelper
    {
        /// <summary>
        /// Used by <see cref="Endpoints{TVertex}.ToString"/> to reduce generic code.
        /// </summary>
        internal static string PairToString(string tail, string head)
        {
            var s = new StringBuilder();
            s.Append('[');

            if (tail != null)
                s.Append(tail);

            s.Append(", ");

            if (head != null)
                s.Append(head);

            s.Append(']');

            return s.ToString();
        }
    }
}
