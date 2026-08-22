# Project Structure

## Layout

- `src/` — one library project per package.
- `src/Shared/` — shared projects (`.shproj`, `.projitems`) with helpers and polyfills, imported by target framework condition.
- `tests/`, `benchmarks/`, `samples/` — one project per component under test, measurement, or demonstration.
- `docs/` — documentation, with ADRs in `docs/decisions/`.
- `assets/` — icon, signing key.
- `tools/` — development utilities and agent configuration.

## Naming

- **Projects**: the project file and its folder share one name.
- **Namespaces**: `Arborescence.[Component]`, under the root namespace `Arborescence`.
- **Prefixes**: `Tests.` and `Benchmarks.` plus the component, `Samples.` plus the component and the purpose.
- **Suffix**: `.Specialized` for the `Int32` counterpart of a component.

## Configuration Files

- `Directory.Build.props` — layered: the root file holds the repository-wide properties, and the file in a top-level folder imports it and adds its own.
- `Directory.Packages.props` — central package versions.
- `global.json` — SDK version.
- `nuget.config` — package sources.
- `.editorconfig` — code style.
