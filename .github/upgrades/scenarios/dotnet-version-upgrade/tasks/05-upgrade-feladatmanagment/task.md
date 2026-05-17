# 05-upgrade-feladatmanagment: Upgrade FeladatManagment

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
