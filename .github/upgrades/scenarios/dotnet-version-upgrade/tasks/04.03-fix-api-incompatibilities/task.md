# 04.03-fix-api-incompatibilities: Fix source and binary API incompatibilities for .NET 10.0

# 04.03-fix-api-incompatibilities: Fix API-Level Incompatibilities

## Objective
Resolve 547 source-level API incompatibilities and 8 binary incompatibilities introduced by .NET 10.0 upgrade.

## Scope
- Source incompatibilities: APIs that require code changes or use of different methods
- Binary incompatibilities: APIs that require recompilation due to signature changes
- Affected files: 64 Razor/C# files
- All platform targets (android, maccatalyst, windows)

## Known Patterns
- Many incompatibilities likely relate to Blazor/Razor APIs (project contains many .razor files)
- MAUI-specific platform considerations per platform target
- Potential null-safety and type-checking differences from .NET 9 → 10

## Prerequisites
- 04.01-update-maui-tfm completed (TFMs updated)
- 04.02-update-maui-packages completed (packages updated)

## Done When
- Solution builds successfully
- All CS error codes resolved (no CS0* errors)
- Zero build errors on all platform targets
- Zero warnings (or documented pre-existing warnings)
- All API incompatibilities addressed
