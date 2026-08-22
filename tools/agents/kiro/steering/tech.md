# Technology Stack

## Platform

- **SDK**: @global.json
- **Target frameworks**: `src/` projects multi-target down to `net462` and `netstandard2.0`.
    Read the `.csproj` before you use a recent API, and polyfill it in `src/Shared/` instead of raising the floor.
- **Language**: C# 12 in `src/`, C# 14 in `tests/` and `benchmarks/`.
- **Nullable reference types**: enabled everywhere.

## Rules

- Declare package versions in `Directory.Packages.props` only, never in a `.csproj`.
- Document every public API; `GenerateDocumentationFile` reports a missing comment as a warning.
- Fix analyzer warnings instead of suppressing them: `AnalysisMode` is `All`.
- Place using directives inside the namespace.
- Keep the sections of `.editorconfig` sorted alphabetically.
- Record a notable decision as an ADR in `docs/decisions/`.

## Tools

- **xUnit** for tests.
- **BenchmarkDotNet** for benchmarks.
    Run them in Release:
    ```bash
    dotnet run --project benchmarks/Benchmarks.Traversal --configuration Release
    ```
- **DotNet.ReproducibleBuilds** for deterministic builds.
    `src/` assemblies are signed with `assets/arborescence.snk`, and the build writes to `.artifacts/`.
