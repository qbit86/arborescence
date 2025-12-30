---
status: superseded by [ADR-0007](0007-restrict-language-version-to-csharp-12)
date: 2025-06-02
---
# Restrict language version to C# 10

## Context and Problem Statement

Which language version should we prefer?
Should we restrict the version at all?

## Decision Drivers

- Improved pattern matching and record types enhancements
- Binary compatible with Unity 6
- Source compatible with Unity 6
- Source compatible with online coding platforms: CodinGame[^CG], LeetCode[^LC], HackerRank[^HR], Codeforces[^CF]

## Considered Options

- C# 9[^C9]
- C# 10[^C10]
- C# 11[^C11]
- C# 12[^C12]

## Decision Outcome

Chosen option: "C# 10".
This is specified in one of the _Directory.Build.props_ files:
```xml
<Project>
  <PropertyGroup>
    <LangVersion>10</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

### Consequences

- Good, because C# 10 is supported by all the popular online coding platforms.
- Good, because C# 10 is both binary and source code compatible with Unity 6.

## Pros and Cons of the Options

### C# 9

- Bad, because Unity 6 partially supports C# 10, so we can leverage newer features.

### C# 10

- Good, because is supported by Unity 6.
- Good, because is supported by all the popular online coding platforms.

### C# 11

- Good, because provides additional language improvements.
- Bad, because is not supported by Unity 6.
- Bad, because is not supported by Codeforces[^CF] as of June 2025.

## More Information

- C# language versioning  
    https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
- .NET Standard  
    https://docs.microsoft.com/en-us/dotnet/standard/net-standard

[^C9]: What's new in C# 9.0  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9
[^C10]: What's new in C# 10  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10
[^C11]: What's new in C# 11  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11
[^C12]: What's new in C# 12  
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12
[^CF]: Codeforces Command Lines  
    https://codeforces.com/blog/entry/121114
[^CG]: CodinGame - Languages Versions  
    https://codingame.com/playgrounds/40701/help-center/languages-versions
[^HR]: HackerRank - Execution Environment and Samples  
    https://support.hackerrank.com/articles/6693750503-execution-environment
[^LC]: LeetCode - What are the environments for the programming languages?  
    https://support.leetcode.com/hc/en-us/articles/360011833974-What-are-the-environments-for-the-programming-languages-
