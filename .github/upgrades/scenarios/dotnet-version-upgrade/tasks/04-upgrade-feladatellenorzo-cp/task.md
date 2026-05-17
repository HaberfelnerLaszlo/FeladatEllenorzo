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
