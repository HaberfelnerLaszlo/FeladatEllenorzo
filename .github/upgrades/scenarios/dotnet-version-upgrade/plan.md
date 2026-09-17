# .NET 10.0 Upgrade Plan

## Overview

**Target**: Upgrade FeladatEllenorzo solution from .NET 9 to .NET 10.0

**Scope**: 5 projects (3 ASP.NET Core, 1 MAUI application, 1 legacy tool)
- 41 affected files
- 656 total issues identified (16 mandatory, 640 potential)
- Primary concerns: Source-level API incompatibilities (548 occurrences), behavioral changes (78 occurrences), NuGet package updates

**Strategy**: Bottom-Up (dependency-driven)
- Upgrade foundation libraries first (FeladatLibrary)
- Upgrade independent services (Data_Api, M365Agent)
- Upgrade dependent applications (FeladatEllenorzo_CP, FeladatManagment)

## Tasks

### 01-upgrade-feladatlibrary

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

---

### 02-upgrade-data-api

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

---

### 03-upgrade-m365agent

Upgrade M365Agent.atkproj (.NET tool) to .NET 10.0. Independent application with no solution dependencies.

**Key concerns:**
- Currently shows 0 issues (lowest risk)
- May still have package-level incompatibilities not yet detected
- Likely the fastest upgrade in the plan

**Done when:**
- Project file targets net10.0 (if applicable to .atkproj format)
- Project builds successfully
- Tool functionality verified

---

### 04-upgrade-feladatellenorzo-cp

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

---

### 05-upgrade-feladatmanagment

Upgrade FeladatManagment.csproj (ASP.NET Core service) to .NET 10.0. This is a dependent project that uses FeladatLibrary.

**Key concerns:**
- 31 issues with 4 mandatory
- Depends on FeladatLibrary completion
- Source-level API incompatibilities (548 total in solution, some likely here)
- Binary incompatibilities may require recompilation

**Done when:**
- Project file targets net10.0
- All source and binary incompatibilities resolved
- All NuGet packages updated
- Solution builds without errors
- Depends on 01-upgrade-feladatlibrary completion

---

### 06-full-solution-validation

Final validation of the complete upgraded solution.

**Key concerns:**
- Integration testing across all projects
- Potential breaking change interactions between upgraded components
- Multi-project build consistency

**Done when:**
- Full solution builds without errors or warnings
- All tests pass
- No regressions detected
- All packages are updated and compatible
