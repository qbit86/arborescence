# Technology Stack

## Build System & Framework

- **.NET 9.0**: Target framework (SDK version 9.0.100)
- **MSBuild**: Project system with SDK-style .csproj files
- **Central Package Management**: Uses Directory.Packages.props for version management
- **Artifacts Output**: Uses modern artifacts output structure

## Development Tools

- **xUnit**: Testing framework (v2.6.5)
- **BenchmarkDotNet**: Performance benchmarking (v0.13.12)
- **Microsoft.NET.Test.Sdk**: Test runner (v17.8.0)
- **SourceLink**: Source debugging support

## Code Quality & Analysis

- **Nullable Reference Types**: Enabled project-wide
- **Code Analysis**: Latest analysis level with all analyzers enabled
- **Deterministic Builds**: Enabled for reproducible builds
- **EditorConfig**: Comprehensive code style enforcement

## Common Commands

### Building

```bash
dotnet build
dotnet build --configuration Release
```

### Testing

```bash
dotnet test
dotnet test --configuration Release
```

### Packaging

```bash
dotnet pack
dotnet pack --configuration Release
```

### Benchmarking

```bash
dotnet run --project benchmarks/Benchmarks.Traversal --configuration Release
```

### Running Samples

```bash
dotnet run --project samples/Samples.Traversal.BasicUsage
```

## Project Configuration

- **Root Namespace**: Arborescence
- **Neutral Language**: en-US
- **Assembly Signing**: Uses arborescence.snk key file
- **NuGet Configuration**: Custom nuget.config for package sources
