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
