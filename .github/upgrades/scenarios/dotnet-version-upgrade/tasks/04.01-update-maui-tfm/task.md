# 04.01-update-maui-tfm: Update MAUI target frameworks to net10.0 for all platforms

# 04.01-update-maui-tfm: Update MAUI Target Frameworks

## Objective
Update FeladatEllenorzo_CP multi-platform target frameworks from net9.0 to net10.0 for all platforms (android, maccatalyst, windows).

## Scope
- Update TargetFrameworks property in FeladatEllenorzo_CP.csproj
- All platform-specific conditional properties that reference the old TFMs
- Maintain platform versions (android35.0, maccatalyst26.2, windows10.0.26100.0)

## Key Concerns
- MAUI projects use conditional TFM assignments based on OS platform detection
- All platform variants must be updated consistently
- Windows package version may need adjustment (windows10.0.26100.0 should become windows10.0.26100.0 for .NET 10)

## Done When
- TargetFrameworks contains net10.0-android;net10.0-maccatalyst;net10.0-windows10.0.26100.0
- All conditional TargetFramework assignments updated
- Project loads without framework-related errors
- Ready for package updates (next subtask)
