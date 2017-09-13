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
                // .With(BenchmarkDotNet.Environments.Jit.RyuJit)
                // .With(BenchmarkDotNet.Environments.Platform.X64)
                // .With(BenchmarkDotNet.Environments.Runtime.Clr)
                .ApplyAndFreeze(RunMode.Short)
                ;
            IConfig config = ManualConfig.Create(DefaultConfig.Instance)
                .With(job);

            Summary insertEnumSummary = BenchmarkDotNet.Running.BenchmarkRunner.Run<DfsBenchmark>(config);
        }
    }
}
