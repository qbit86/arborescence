# Ubiquitous Graphs

Generic graph library inspired by [Concept C#: Type Classes for the Masses](https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md).

|          Method | VertexCount |             Mean |          Error |        StdDev | Scaled |     Gen 0 |    Gen 1 | Allocated |
|---------------- |------------ |-----------------:|---------------:|--------------:|-------:|----------:|---------:|----------:|
| **BaselineDfsTree** |          **10** |        **24.222 us** |     **13.0331 us** |     **0.7364 us** |   **1.00** |    **1.8005** |        **-** |    **5720 B** |
|  DefaultDfsTree |          10 |         9.902 us |      0.1164 us |     0.0066 us |   0.41 |    0.2441 |        - |     816 B |
|  CachingDfsTree |          10 |         8.893 us |      3.3281 us |     0.1880 us |   0.37 |    0.0916 |        - |     312 B |
| **BaselineDfsTree** |         **100** |    **10,644.551 us** |    **718.1624 us** |    **40.5775 us** |   **1.00** |   **46.8750** |        **-** |  **190880 B** |
|  DefaultDfsTree |         100 |       359.791 us |     10.1720 us |     0.5747 us |   0.03 |    2.4414 |        - |    8952 B |
|  CachingDfsTree |         100 |       307.204 us |      3.5048 us |     0.1980 us |   0.03 |         - |        - |     312 B |
| **BaselineDfsTree** |        **1000** | **4,491,136.410 us** | **19,681.4054 us** | **1,112.0359 us** |   **1.00** | **2375.0000** | **375.0000** | **7546992 B** |
|  DefaultDfsTree |        1000 |    14,054.296 us |  2,004.2414 us |   113.2434 us |   0.00 |   15.6250 |        - |   69968 B |
|  CachingDfsTree |        1000 |    12,060.505 us |  2,247.3279 us |   126.9782 us |   0.00 |         - |        - |     312 B |
