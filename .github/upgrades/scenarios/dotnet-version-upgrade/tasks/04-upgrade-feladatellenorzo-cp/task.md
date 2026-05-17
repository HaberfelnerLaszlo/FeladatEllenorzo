# 04-upgrade-feladatellenorzo-cp: Upgrade FeladatEllenorzo_CP

Upgrade FeladatEllenorzo_CP.csproj (MAUI application) to .NET 10.0. This is a dependent project that uses FeladatLibrary.

**Key concerns:**
- Highest issue count: 561 issues with 9 mandatory
- Multi-targeting project (net9.0-android, net9.0-maccatalyst, net9.0-windows)
- Source-level API incompatibilities (significant work expected)
- Behavioral changes requiring validation
- MAUI platform-specific considerations

**Done when:**
- Project file targets net10.0 for all platforms (android, maccatalyst, windows)
- All source-level API incompatibilities resolved
- All NuGet packages updated
- Solution builds without errors
- Depends on 01-upgrade-feladatlibrary completion

## Scope Inventory

### Project Details
- **File**: FeladatEllenorzo_CP/FeladatEllenorzo_CP.csproj
- **Project Type**: .NET MAUI Application (multi-platform)
- **Current TFMs**: net9.0-android, net9.0-maccatalyst, net9.0-windows10.0.26100.0
- **Target TFMs**: net10.0-android, net10.0-maccatalyst, net10.0-windows10.0.26100.0
- **Total Files**: 64 files affected
- **CPM**: Enabled (versions in Directory.Packages.props)

### Project Dependencies
- **FeladatLibrary** (local) — ✅ Already upgraded to net10.0
- **17 NuGet packages** (see below)

### Package Status
**Requires Updates:**
- Microsoft.AspNetCore.Components: 9.0.16 → **10.0.8**
- Microsoft.AspNetCore.Components.Forms: 9.0.16 → **10.0.8**
- Microsoft.AspNetCore.Components.Web: 9.0.16 → **10.0.8**
- System.Text.Json: 9.0.16 → **10.0.8**

**Compatible (no update needed):**
- Azure.Identity (1.19.0) ✅
- CommunityToolkit.Maui (12.2.0) ✅
- Microsoft.Maui.Controls (9.0.110) ✅
- Microsoft.Maui.Controls.Compatibility (9.0.110) ✅
- Microsoft.AspNetCore.Components.WebView.Maui (9.0.110) ✅
- Microsoft.FluentUI.AspNetCore.Components (4.12.1) ✅
- Microsoft.FluentUI.AspNetCore.Components.Icons (4.12.1) ✅
- Microsoft.Graph (5.93.0) ✅
- Microsoft.IdentityModel.JsonWebTokens (8.14.0) ✅
- System.IdentityModel.Tokens.Jwt (8.14.0) ✅
- System.Private.Uri (4.3.2) ✅
- TimeZoneConverter (7.0.0) ✅
- Newtonsoft.Json (13.0.4) ✅

### Issue Breakdown
- **Mandatory**: 9 issues
  - Project.0002 (TFM needs to change): 1
  - Api.0001 (Binary incompatibilities): 8
- **Potential**: 552 issues
  - Api.0002 (Source incompatibilities): 547
  - Api.0003 (Behavioral changes): 1
  - NuGet.0002 (Package upgrade recommendations): 4

### Complexity Assessment

**This task requires decomposition** due to:
1. **High issue count** (561 issues across 64 files)
2. **Multiple concerns** (TFM update, package updates, API fixes)
3. **Multi-platform complexity** (Android, macCatalyst, Windows require per-platform validation)
4. **Source-level changes** (547 API incompatibilities need individual fixes)
5. **Failure blast radius** (large number of affected files increases risk)

**Recommended subtasks:**
1. Update target frameworks for all platforms
2. Update NuGet packages to .NET 10.0 compatible versions
3. Fix API-level incompatibilities (source and binary)
