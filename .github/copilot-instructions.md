# UnlimitedParty — Copilot Instructions

## Project Overview

Bannerlord mod that removes companion and clan party limits by patching `DefaultClanTierModel` with Harmony. No game files are modified — patches apply on load and revert on unload.

## Tech Stack

- **Language:** C# 9.0 targeting .NET Framework 4.7.2
- **Game SDK:** TaleWorlds Mount & Blade II: Bannerlord (TaleWorlds.Core, TaleWorlds.CampaignSystem, TaleWorlds.Library, TaleWorlds.MountAndBlade)
- **Patching:** Harmony 2.2.2 for runtime method patching
- **Testing:** xUnit 2.6.6
- **Nullable:** Disabled project-wide

## Build, Test & Deploy Commands

```powershell
# Build
dotnet build src\UnlimitedParty\UnlimitedParty.csproj -c Release

# Run tests
dotnet test tests\UnlimitedParty.Tests\UnlimitedParty.Tests.csproj

# Deploy to game
./deploy.ps1
```

## Architecture

| File | Role |
|------|------|
| `SubModule.cs` | Mod entry point — applies all Harmony patches on load, reverts on unload |
| `Patches/CompanionLimitPatch.cs` | Harmony prefix patch overriding `DefaultClanTierModel.GetCompanionLimit` to return 9999 |
| `Patches/PartyLimitPatch.cs` | Harmony prefix patch overriding `DefaultClanTierModel.GetPartyLimitForTier` to return 9999 |

### Key Design Decisions

- **Prefix patches returning `false`:** Each patch sets `__result = 9999` and returns `false` to skip the original method entirely. This is the simplest Harmony strategy for replacing a return value.
- **Single Harmony ID:** All patches share `"com.unlimitedparty.bannerlord"` for clean bulk unpatching.
- **Overridden class:** `DefaultClanTierModel` — any other mod patching the same `GetCompanionLimit` or `GetPartyLimitForTier` methods may conflict.

## Code Conventions

- **Namespace:** `UnlimitedParty` at root, `UnlimitedParty.Patches` for Harmony patches
- **XML documentation:** All public and internal types and methods must have `<summary>`, `<param>`, and `<returns>` XML doc comments
- **Patch classes:** Marked `internal static`, one class per patched method, each in its own file
- **Harmony attributes:** Use `[HarmonyPatch]` with explicit type arrays for argument types
- **Error handling:** `SubModule.OnSubModuleLoad` wraps `PatchAll()` in try-catch and displays colored messages via `InformationManager.DisplayMessage`

## Module Metadata

`Module/SubModule.xml` defines the mod for Bannerlord's launcher:
- **Id:** `UnlimitedParty`
- **Dependencies:** Native, SandBoxCore, Sandbox, StoryMode
- **Entry point:** `UnlimitedParty.SubModule`

Keep `SubModule.xml` in sync with any namespace or class name changes.

## Post-Change Workflow

After making any code changes, always follow these steps in order:

1. **Build:** `dotnet build src\UnlimitedParty\UnlimitedParty.csproj -c Release` — confirm the project compiles
2. **Write tests:** Add or update tests covering the new or changed code
3. **Test:** `dotnet test tests\UnlimitedParty.Tests\UnlimitedParty.Tests.csproj` — confirm all tests pass before proceeding
4. **Deploy:** `./deploy.ps1` — copies the built DLL and SubModule.xml to the game's module folder

Do not deploy if the build fails or any tests are failing.

## Testing Guidelines

- Tests use `InternalsVisibleTo` to access `internal` patch classes
- Patch prefix methods are tested directly by calling `Prefix(ref int __result)` and asserting the result is 9999 and return value is false
- Harmony attributes are validated via reflection to ensure patches target the correct `DefaultClanTierModel` methods with correct argument types
