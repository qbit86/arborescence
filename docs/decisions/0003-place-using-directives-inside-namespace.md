# Place using directives inside a namespace

## Context and Problem Statement

Should using directives be inside or outside a namespace?

## Decision Drivers

* Easy to concatenate files

## Considered Options

* Place using directives outside a namespace
* Place using directives inside a namespace

## Decision Outcome

Chosen option: “Place using directives inside a namespace”, because this allows to concatenate files into a single amalgam. Otherwise the compiler complains: “[CS1529] A using clause must precede all other elements defined in the namespace except extern alias declarations”.

It can be useful in some programming competitions where you have to consume a library in a single source file.

This decision is enforced with a setting in an EditorConfig file:

    csharp_using_directive_placement = inside_namespace:suggestion

## Pros and Cons of the Options

### Place using directives outside a namespace

```csharp
using System;

namespace Foo
{
    ...
}
```

* Good, because is conventional

### Place using directives inside a namespace

```csharp
namespace Foo
{
    using System;

    ...
}
```

* Good, because allows to concatenate several files
* Bad, because is less common

## References

* https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/formatting-rules#using-directive-options
* https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/language-rules#csharp_using_directive_placement
* https://jetbrains.com/help/rider/Using_EditorConfig.html
* https://stackoverflow.com/questions/125319/should-using-directives-be-inside-or-outside-the-namespace
