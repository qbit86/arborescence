---
status: accepted
date: 2022-11-16
---
# Restrict language version to C# 9

## Context and Problem Statement

Which language version should we prefer? Should we restrict the version at all?

## Decision Drivers

* Target-typed new expressions and improved pattern matching
* Binary compatible with Unity 2021.3
* Source compatible with Unity 2021.3
* Source compatible with online coding platforms: CodinGame[^CG], LeetCode[^LC], HackerRank[^HR], Codeforces[^CF]

## Considered Options

* C# 8[^C8]
* C# 9[^C9]
* C# 10[^C10]
* C# 11[^C11]

## Decision Outcome

Chosen option: “C# 9”.
This is specified in one of the _Directory.Build.props_ files:
```xml
<Project>
  <PropertyGroup>
    <LangVersion>9</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

### Consequences

* Good, because C# 9 is supported by the most popular online coding platforms.
* Good, because C# 9 is both binary (via .NET Standard 2.1) and source code compatible with Unity 2021.3.

## Pros and Cons of the Options

### C# 8

* Bad, because is too outdated.

### C# 10

* Good, because is supported by the most popular online coding platforms.
* Bad, because is not supported by Unity 2021.3.

### C# 11

* Bad, because is not supported by some popular online coding platforms as of November 2022.

## More Information

- C# language versioning  
    https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
- .NET Standard  
    https://docs.microsoft.com/en-us/dotnet/standard/net-standard

[^C8]: What's new in C# 8.0  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history#c-version-80
[^C9]: What's new in C# 9.0  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9
[^C10]: What's new in C# 10  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10
[^C11]: What's new in C# 11  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11
[^CF]: Codeforces Command Lines  
    https://codeforces.com/blog/entry/121114
[^CG]: CodinGame - Languages Versions  
    https://codingame.com/playgrounds/40701/help-center/languages-versions
[^HR]: HackerRank - Execution Environment and Samples  
    https://support.hackerrank.com/hc/en-us/articles/1500002392722--Execution-Environment-and-Samples
[^LC]: LeetCode - What are the environments for the programming languages?  
    https://support.leetcode.com/hc/en-us/articles/360011833974-What-are-the-environments-for-the-programming-languages-
