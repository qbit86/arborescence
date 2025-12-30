---
status: accepted
date: 2025-12-31
---
# Restrict language version to C# 12

## Context and Problem Statement

Which language version should we prefer?
Should we restrict the version at all?

## Decision Drivers

- Primary constructors and collection expressions
- Binary compatible with Unity 6
- Source compatible with Unity 6
- Source compatible with online coding platforms: CodinGame[^CG], LeetCode[^LC], HackerRank[^HR], Codeforces[^CF]

## Considered Options

- C# 10[^C10]
- C# 11[^C11]
- C# 12[^C12]
- C# 13[^C13]

## Decision Outcome

Chosen option: "C# 12".
This is specified in one of the _Directory.Build.props_ files:
```xml
<Project>
  <PropertyGroup>
    <LangVersion>12</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

### Consequences

- Good, because C# 12 is supported by all the popular online coding platforms.
- Good, because primary constructors reduce boilerplate code.
- Good, because collection expressions provide cleaner syntax for initializing collections.

## Pros and Cons of the Options

### C# 10

- Good, because C# 10 is both binary and source code compatible with Unity 6.
- Bad, because it lacks primary constructors and collection expressions available in newer versions.

### C# 11

- Good, because it provides raw string literals and generic math support.

### C# 12

- Good, because it is supported by all the popular online coding platforms.
- Good, because primary constructors simplify class and struct definitions.
- Good, because collection expressions provide a unified syntax for collection initialization.

### C# 13

- Good, because it provides additional language improvements.
- Bad, because it is not yet widely supported by online coding platforms as of December 2025.

## More Information

- C# language versioning
    https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
- .NET Standard
    https://docs.microsoft.com/en-us/dotnet/standard/net-standard

[^C10]: What's new in C# 10
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10
[^C11]: What's new in C# 11
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11
[^C12]: What's new in C# 12
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12
[^C13]: What's new in C# 13
    https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13
[^CF]: Codeforces Command Lines
    https://codeforces.com/blog/entry/121114
[^CG]: CodinGame - Languages Versions
    https://codingame.com/playgrounds/40701/help-center/languages-versions
[^HR]: HackerRank - Execution Environment and Samples
    https://support.hackerrank.com/articles/6693750503-execution-environment
[^LC]: LeetCode - What are the environments for the programming languages?
    https://support.leetcode.com/hc/en-us/articles/360011833974-What-are-the-environments-for-the-programming-languages-
