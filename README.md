# Arborescence Graph Library

Generic graph library inspired by [Concept C#: Type Classes for the Masses](https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md).

API structure is partially influenced by [Boost Graph Concepts](https://www.boost.org/doc/libs/1_68_0/libs/graph/doc/graph_concepts.html).

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.836 (1909/November2018Update/19H2)
Intel Core i5-4690K CPU 3.50GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-OEUNEA : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

IterationCount=3  LaunchCount=1  WarmupCount=3  
```

|                    Method | VertexCount |             Mean |           Error |        StdDev |    Ratio |  RatioSD |      Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------- |------------ |-----------------:|----------------:|--------------:|---------:|---------:|-----------:|------:|------:|----------:|
|        **InstantDfsTree** |      **10** |     **1.067 μs** |   **0.2498 μs** | **0.0137 μs** | **1.00** | **0.00** | **0.0076** | **-** | **-** |  **24 B** |
|    EnumerableDfsEdgesTree |          10 |         3.534 μs |       0.1693 μs |     0.0093 μs |     3.31 |     0.04 |     0.0076 |     - |     - |      32 B |
| EnumerableDfsVerticesTree |          10 |         1.433 μs |       0.3085 μs |     0.0169 μs |     1.34 |     0.03 |     0.0095 |     - |     - |      32 B |
|                           |             |                  |                 |               |          |          |            |       |       |           |
|        **InstantDfsTree** |     **100** |    **32.672 μs** |   **5.8880 μs** | **0.3227 μs** | **1.00** | **0.00** |      **-** | **-** | **-** |  **24 B** |
|    EnumerableDfsEdgesTree |         100 |       124.857 μs |       1.2744 μs |     0.0699 μs |     3.82 |     0.04 |          - |     - |     - |      32 B |
| EnumerableDfsVerticesTree |         100 |        31.053 μs |       1.7995 μs |     0.0986 μs |     0.95 |     0.01 |          - |     - |     - |      32 B |
|                           |             |                  |                 |               |          |          |            |       |       |           |
|        **InstantDfsTree** |    **1000** | **1,289.778 μs** | **132.4713 μs** | **7.2612 μs** | **1.00** | **0.00** |      **-** | **-** | **-** |  **24 B** |
|    EnumerableDfsEdgesTree |        1000 |     5,119.723 μs |     282.3477 μs |    15.4764 μs |     3.97 |     0.02 |          - |     - |     - |      32 B |
| EnumerableDfsVerticesTree |        1000 |     1,015.236 μs |      33.6826 μs |     1.8463 μs |     0.79 |     0.00 |          - |     - |     - |      32 B |
