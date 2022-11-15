# Restrict language version to C# 8

* Status: accepted
* Date: 2021-06-19

## Context and Problem Statement

Which language version should we prefer? Should we restrict the version at all?

## Decision Drivers

* Nullable reference types
* Source compatible with Unity 2018.4
* Binary compatible with Unity 2018.4
* Source compatible with Unity 2020.3
* Source compatible with online coding platforms like CodinGame or LeetCode

## Considered Options

* Latest available language version
* C# 7.3
* C# 8
* C# 9

## Decision Outcome

Chosen option: “C# 8”.
This is specified in one of the Directory.Build.props files:
```xml
<Project>
  <PropertyGroup>
    <LangVersion>8</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

The main reason to ditch 7.3 is that it complicates nullable annotations.

The latest available compiler version C# 9 is not supported by some popular online coding platforms as of June 2021.

### Negative Consequences

- C# 8 is not source-compatible with Unity 2018.4.
However it is binary-compatible via .NET Standard 2.0.
Unity 2020.3 partially supports C# 8.

## See also

- C# language versioning  
    https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
- What's new in C# 8.0  
    https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
- .NET Standard  
    https://docs.microsoft.com/en-us/dotnet/standard/net-standard
- Nullable reference types  
    https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references  
    https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis  
    https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/nullable-reference-types  
    https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/upgrade-to-nullable-references
- Languages Versions  
    https://codingame.com/playgrounds/40701/help-center/languages-versions  
    https://support.leetcode.com/hc/en-us/articles/360011833974-What-are-the-environments-for-the-programming-languages-  
    https://support.hackerrank.com/hc/en-us/articles/1500002392722--Execution-Environment-and-Samples
