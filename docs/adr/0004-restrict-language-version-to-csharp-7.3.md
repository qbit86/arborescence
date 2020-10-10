# Restrict language version to C# 7.3

* Status: accepted
* Date: 2020-10-10

## Context and Problem Statement

Which language version should we prefer? Should we restrict the version at all?

## Decision Drivers

* Binary compatibile with Unity 2018.4
* Source compatibile with Unity 2018.4
* Source compatibile with online coding platforms like CodinGame or HackerRank

## Considered Options

* Latest available language version
* C# 7.3
* C# 8.0

## Decision Outcome

Chosen option: “C# 7.3”.

“C# 8.0” would also work, but it [doesn't add](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8) much value for this particular library comparing to C# 7.3.
It's probably worth keeping source compatibility (C# 7.3) with Unity 2018.4, even if binary compatibility (.NET Standard 2.0) would be sufficient.

The latest available compiler version C# 9.0 is not supported by some popular online coding platforms as of October 2020.

### Positive Consequences <!-- optional -->

* [e.g., improvement of quality attribute satisfaction, follow-up decisions required, …]
* …

### Negative Consequences <!-- optional -->

* [e.g., compromising quality attribute, follow-up decisions required, …]
* …

## Pros and Cons of the Options <!-- optional -->

### [option 1]

[example | description | pointer to more information | …] <!-- optional -->

* Good, because [argument a]
* Good, because [argument b]
* Bad, because [argument c]
* … <!-- numbers of pros and cons can vary -->

### [option 2]

[example | description | pointer to more information | …] <!-- optional -->

* Good, because [argument a]
* Good, because [argument b]
* Bad, because [argument c]
* … <!-- numbers of pros and cons can vary -->

### [option 3]

[example | description | pointer to more information | …] <!-- optional -->

* Good, because [argument a]
* Good, because [argument b]
* Bad, because [argument c]
* … <!-- numbers of pros and cons can vary -->

## Links <!-- optional -->

* [Link type] [Link to ADR] <!-- example: Refined by [ADR-0005](0005-example.md) -->
* … <!-- numbers of links can vary -->

## References

* https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version
* https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
* https://docs.microsoft.com/en-us/dotnet/standard/net-standard
