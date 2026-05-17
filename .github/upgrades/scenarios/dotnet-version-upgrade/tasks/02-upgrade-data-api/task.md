# 02-upgrade-data-api: Upgrade Data_Api

Upgrade Data_Api.csproj (ASP.NET Core service) to .NET 10.0. This is an independent service with no solution dependencies.

**Key concerns:**
- 4 issues with 2 mandatory
- Likely NuGet package incompatibilities — may need version updates
- ASP.NET Core specific APIs may have breaking changes

**Done when:**
- Project file targets net10.0
- All NuGet packages updated
- Project builds successfully
- No unresolved NuGet incompatibilities

## Scope Inventory

### Project Details
- **File**: Data_Api/Data_Api.csproj
- **Current TFM**: net9.0
- **Target TFM**: net10.0
- **Project Type**: ASP.NET Core Web API (SDK-style)
- **Total Files**: 32
- **CPM**: Enabled (versions in Directory.Packages.props)

### Package Status
- **EfCore.SchemaCompare** (9.0.0) — ✅ Compatible, no update needed
- **Microsoft.EntityFrameworkCore.InMemory** (9.0.9) — ⚠️ **UPDATE NEEDED** to 10.0.8
- **Microsoft.EntityFrameworkCore.Tools** (9.0.9) — ⚠️ **UPDATE NEEDED** to 10.0.8
- **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** (1.22.1) — ❌ **INCOMPATIBLE** (need to investigate)
- **Pomelo.EntityFrameworkCore.MySql** (9.0.0) — ✅ Compatible, no update needed

### Issue Summary
- **Mandatory**: 2 issues
  - Project.0002 (TFM needs to change)
  - NuGet.0001 (Incompatible package)
- **Potential**: 2 issues
  - NuGet.0002 (Package upgrades recommended)

### Upgrade Steps
1. Update TargetFramework from net9.0 to net10.0
2. Update Entity Framework Core packages to 10.0.8
3. Handle incompatible Azure Containers Tools package (investigate/remove)
4. Build and validate
