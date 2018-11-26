namespace Ubiquitous.Models
{
    public readonly struct Int32AdditiveMonoidPolicy : IMonoidPolicy<int>
    {
        public int Identity => 0;

        public int Combine(int left, int right)
        {
            return left + right;
        }
    }
}
