# Copilot Instructions — UnlimitedParty

These instructions apply to the `src/UnlimitedParty/UnlimitedParty.csproj` project and its tests.

## Project Purpose

UnlimitedParty removes companion and clan party limits by Harmony-patching `DefaultClanTierModel` methods.

## Scope Rules

- Keep changes limited to `UnlimitedParty` and `UnlimitedParty.Tests` unless explicitly requested.
- Do not modify game files directly.
- Prefer patch-level changes over broad behavioral rewrites.

## Architecture

- `src/UnlimitedParty/SubModule.cs`: module lifecycle and Harmony apply/unpatch.
- `src/UnlimitedParty/Patches/CompanionLimitPatch.cs`: forces companion limit.
- `src/UnlimitedParty/Patches/PartyLimitPatch.cs`: forces party limit.

## Coding Conventions

- Target framework and style must stay compatible with `.NET Framework 4.7.2` and C# 9.
- Keep patch classes `internal static` and focused on one method each.
- Preserve Harmony ID `com.unlimitedparty.bannerlord` for safe unpatching.
- Keep behavior deterministic: patches should set `__result` and skip original when overriding.

## Testing Requirements

- Add or update tests for every behavior change.
- Verify both result behavior and Harmony target metadata via reflection.
- Keep tests in `tests/UnlimitedParty.Tests` using xUnit.

## Required Validation Flow

Run in order after edits:

1. `dotnet build src/UnlimitedParty/UnlimitedParty.csproj -c Release`
2. `dotnet test tests/UnlimitedParty.Tests/UnlimitedParty.Tests.csproj`
3. `./deploy.ps1` (only after build and tests pass)

## Deployment Expectations

Deployment should produce these files under `<Bannerlord>/Modules/UnlimitedParty/`:

- `Module/SubModule.xml`
- `bin/Win64_Shipping_Client/UnlimitedParty.dll`
- `bin/Win64_Shipping_Client/0Harmony.dll`
