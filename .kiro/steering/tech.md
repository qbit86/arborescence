# Technology Stack

## Build System & Framework

- **.NET**: @global.json
- **MSBuild**: Project system with SDK-style .csproj files
- **Central Package Management**: Uses Directory.Packages.props for version management
- **Artifacts Output**: Uses modern artifacts output structure

## Development Tools

- **xUnit**: Testing framework
- **BenchmarkDotNet**: Performance benchmarking
- **Microsoft.NET.Test.Sdk**: Test runner
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
