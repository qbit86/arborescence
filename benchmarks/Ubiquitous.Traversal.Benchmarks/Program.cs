namespace Ubiquitous
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Reports;

    internal static class Program
    {
        private static void Main()
        {
            // http://benchmarkdotnet.org/Configs/Configs.htm
            var job = new Job(Job.Default)
                .ApplyAndFreeze(RunMode.Short);

            IConfig config = ManualConfig.Create(DefaultConfig.Instance)
                .With(job);

            Summary unused = BenchmarkDotNet.Running.BenchmarkRunner.Run<DfsTreeBoostBenchmark>(config);
        }
    }
}
