namespace Ubiquitous
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Reports;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            // http://benchmarkdotnet.org/Configs/Configs.htm
            Job job = new Job(Job.Default)
                .ApplyAndFreeze(RunMode.Short);

            IConfig config = ManualConfig.Create(DefaultConfig.Instance)
                .With(job);

            Summary insertEnumSummary = BenchmarkDotNet.Running.BenchmarkRunner.Run<DfsTreeBoostBenchmark>(config);
        }
    }
}
