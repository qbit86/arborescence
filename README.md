# Ubiquitous Graphs

Generic graph library inspired by [Concept C#: Type Classes for the Masses](https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md).

```
BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.192)
Intel Core i5-4690K CPU 3.50GHz (Haswell), 1 CPU, 4 logical cores and 4 physical cores
Frequency=3417977 Hz, Resolution=292.5707 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host]     : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT
  Job-EYCWWJ : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT

LaunchCount=1  TargetCount=3  WarmupCount=3
```

|          Method | VertexCount |             Mean |          Error |        StdDev | Scaled |     Gen 0 |    Gen 1 | Allocated |
|---------------- |------------ |-----------------:|---------------:|--------------:|-------:|----------:|---------:|----------:|
| **BaselineDfsTree** |          **10** |        **18.768 us** |      **0.4086 us** |     **0.0231 us** |   **1.00** |    **1.8005** |        **-** |    **5720 B** |
|  DefaultDfsTree |          10 |         7.910 us |      0.1075 us |     0.0061 us |   0.42 |    0.1678 |        - |     536 B |
|  CachingDfsTree |          10 |         7.953 us |      0.1522 us |     0.0086 us |   0.42 |         - |        - |       0 B |
|                 |             |                  |                |               |        |           |          |           |
| **BaselineDfsTree** |         **100** |     **8,498.316 us** |    **177.1048 us** |    **10.0068 us** |   **1.00** |   **46.8750** |        **-** |  **190880 B** |
|  DefaultDfsTree |         100 |       266.875 us |     12.5735 us |     0.7104 us |   0.03 |    2.4414 |        - |    8672 B |
|  CachingDfsTree |         100 |       269.474 us |      5.6755 us |     0.3207 us |   0.03 |         - |        - |       0 B |
|                 |             |                  |                |               |        |           |          |           |
| **BaselineDfsTree** |        **1000** | **3,912,749.136 us** | **86,388.7937 us** | **4,881.1270 us** |  **1.000** | **2375.0000** | **312.5000** | **7546992 B** |
|  DefaultDfsTree |        1000 |    11,085.009 us |    209.4097 us |    11.8320 us |  0.003 |   15.6250 |        - |   69688 B |
|  CachingDfsTree |        1000 |    11,312.123 us |    229.4928 us |    12.9668 us |  0.003 |         - |        - |       0 B |
