# 01-upgrade-feladatlibrary: Upgrade FeladatLibrary

Upgrade the core library (FeladatLibrary.csproj) to .NET 10.0. This is a foundation dependency used by FeladatEllenorzo_CP and FeladatManagment, so it must be completed first.

**Key concerns:**
- 60 issues with 1 mandatory (API binary incompatibility)
- Used by 2 other projects — changes here can affect downstream projects
- Behavioral changes in .NET 10.0 may require validation

**Done when:**
- Project file targets net10.0
- All dependencies updated to .NET 10.0-compatible versions
- Solution builds without errors
- No breaking changes introduced that affect dependents

## Scope Inventory

### Project Details
- **File**: FeladatLibrary/FeladatLibrary.csproj
- **Current TFM**: net9.0
- **Target TFM**: net10.0
- **Project Type**: Razor Class Library (SDK-style)
- **Total Files**: 34
- **Files with Issues**: Multiple files affected by behavioral changes

### Package Status (CPM enabled - versions in Directory.Packages.props)
All packages are currently compatible with .NET 10.0:
- ✅ Blazor-ApexCharts (6.0.2) — compatible
- ✅ Microsoft.AspNetCore.Components (9.0.16) — compatible
- ✅ Microsoft.AspNetCore.Components.Forms (9.0.16) — compatible
- ✅ Microsoft.AspNetCore.Components.Web (9.0.16) — compatible
- ✅ Microsoft.FluentUI.AspNetCore.Components (4.12.1) — compatible
- ✅ Microsoft.FluentUI.AspNetCore.Components.Icons (4.12.1) — compatible
- ✅ Microsoft.Graph (5.93.0) — compatible
- ✅ Newtonsoft.Json (13.0.4) — compatible
- ✅ System.Text.Json (9.0.16) — compatible

### Issue Breakdown
- **Mandatory**: 1 issue (Project.0002 - TFM needs to change)
- **Potential**: 59 issues
  - Api.0003 (Behavioral changes): 55 occurrences
  - NuGet.0002 (Package upgrade recommended): 4 occurrences

### Upgrade Steps
1. Update TargetFramework from net9.0 to net10.0 in project file
2. Verify all packages are compatible (confirmed above — no package changes needed)
3. Build and address any behavioral change issues that arise
4. Validate the full solution still builds
