# Restrict language version to C# 7.3

* Status: accepted
* Date: 2020-10-10

## Context and Problem Statement

Which language version should we prefer? Should we restrict the version at all?

## Decision Drivers

* Binary compatible with Unity 2018.4
* Source compatible with Unity 2018.4
* Source compatible with online coding platforms like CodinGame or HackerRank

## Considered Options

* Latest available language version
* C# 7.3
* C# 8.0

## Decision Outcome

Chosen option: “C# 7.3”.
This is specified in one of the Directory.Build.props files:
```xml
<Project>
  <PropertyGroup>
    <LangVersion>7.3</LangVersion>
  </PropertyGroup>
</Project>
```

“C# 8.0” would also work, but it [doesn't add](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8) much value for this particular library comparing to C# 7.3.
It's probably worth keeping source compatibility (C# 7.3) with Unity 2018.4, even if binary compatibility (.NET Standard 2.0) would be sufficient.
However this option may be used if really necessary.

The latest available compiler version C# 9.0 is not supported by some popular online coding platforms as of October 2020.

### Negative Consequences

* Some useful language features are not available, for example, switch expressions or null-coalescing assignment.
* Nullable reference types are not applicable anyway, because the corresponding attributes are not available in .NET Standard 2.0, there is no even an official NuGet package.

## References

* https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
* https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
* https://docs.microsoft.com/en-us/dotnet/standard/net-standard
