# 03-upgrade-m365agent: Upgrade M365Agent

Upgrade M365Agent.atkproj (.NET tool) to .NET 10.0. Independent application with no solution dependencies.

**Key concerns:**
- Currently shows 0 issues (lowest risk)
- May still have package-level incompatibilities not yet detected
- Likely the fastest upgrade in the plan

**Done when:**
- Project file targets net10.0 (if applicable to .atkproj format)
- Project builds successfully
- Tool functionality verified
