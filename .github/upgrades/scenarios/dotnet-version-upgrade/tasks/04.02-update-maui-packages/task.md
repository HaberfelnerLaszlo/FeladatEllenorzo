# 04.02-update-maui-packages: Update NuGet packages to .NET 10.0 compatible versions

# 04.02-update-maui-packages: Update NuGet Packages

## Objective
Update NuGet packages referenced by FeladatEllenorzo_CP to versions compatible with .NET 10.0.

## Scope
- Update packages in Directory.Packages.props
- Packages to update:
  - Microsoft.AspNetCore.Components: 9.0.16 → 10.0.8
  - Microsoft.AspNetCore.Components.Forms: 9.0.16 → 10.0.8
  - Microsoft.AspNetCore.Components.Web: 9.0.16 → 10.0.8
  - System.Text.Json: 9.0.16 → 10.0.8

## Prerequisite
- 04.01-update-maui-tfm must be completed first (TFM updated)

## Done When
- All four packages updated to their .NET 10.0 versions
- dotnet restore succeeds
- No NU1202 package compatibility errors
- Ready for API-level fixes (next subtask)
