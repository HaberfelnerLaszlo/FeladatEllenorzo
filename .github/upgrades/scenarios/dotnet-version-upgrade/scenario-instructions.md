# .NET 10.0 Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: .NET 10.0 (LTS)

## Source Control
- **Source Branch**: master
- **Working Branch**: dotnet-version-upgrade-main
- **Commit Strategy**: After Each Task

## Strategy
**Selected**: Bottom-Up (Dependency-Driven)
**Rationale**: Solution has clear dependency hierarchy (FeladatLibrary → dependent projects). Upgrading foundation first ensures all downstream projects have compatible dependencies.

### Execution Constraints
- Strict ordering: Foundation libraries (FeladatLibrary) must complete before dependent projects (FeladatEllenorzo_CP, FeladatManagment)
- Between-task validation: Confirm full solution builds after each project upgrade
- Multi-targeting handling: FeladatEllenorzo_CP targets multiple platforms (android, maccatalyst, windows) — all must be updated to net10.0
- MAUI project has highest issue count (561) — validate thoroughly for platform-specific breaking changes
- Commit after each project upgrade (commits already configured in Source Control section)

## Key Decisions Log
- User chose .NET 10.0 (LTS) over preview versions for production stability
