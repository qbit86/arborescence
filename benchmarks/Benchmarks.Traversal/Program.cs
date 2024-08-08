namespace Arborescence;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

internal static class Program
{
    private static void Main()
    {
        // http://benchmarkdotnet.org/Configs/Configs.htm
        var job = new Job(Job.Default)
            .ApplyAndFreeze(RunMode.Short);

        IConfig config = ManualConfig.Create(DefaultConfig.Instance)
            .AddJob(job);

        var _ = BenchmarkRunner.Run<CompactSetBenchmark>(config);
    }
}
