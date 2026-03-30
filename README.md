# Unlimited Party — Bannerlord Mod

Removes the companion and clan party limits, allowing you to recruit as many companions and form as many clan parties as you want.

## Features

- **Unlimited companions:** Raises the companion limit to 9999, so clan tier no longer restricts how many companions you can recruit
- **Unlimited clan parties:** Raises the clan party limit to 9999, letting you send out as many parties as you like
- **Zero configuration:** Just enable the mod and play normally
- **Save compatible:** Can be added to or removed from existing saves

## How It Works

Bannerlord calculates companion and party limits through `DefaultClanTierModel`. By default, these limits scale with your clan tier but remain quite restrictive. This mod uses [Harmony](https://github.com/pardeike/Harmony) patches to:

1. **Override `GetCompanionLimit`** — always returns 9999 instead of the tier-based calculation
2. **Override `GetPartyLimitForTier`** — always returns 9999 instead of the tier-based calculation

## Prerequisites

- **Mount & Blade II: Bannerlord** (Steam or GOG version)
- **.NET Framework 4.7.2 targeting pack** (comes with Visual Studio)
- **Visual Studio 2022** or the **.NET SDK** with `dotnet build` support
- Bannerlord installed at the expected path, or update the path in the `.csproj`

## Project Structure

```
UnlimitedParty/
├── UnlimitedParty.sln
├── deploy.ps1                          # Build & deploy script
├── Module/
│   └── SubModule.xml                   # Bannerlord module descriptor
├── src/
│   └── UnlimitedParty/
│       ├── UnlimitedParty.csproj
│       ├── SubModule.cs                # Mod entry point (Harmony init)
│       └── Patches/
│           ├── CompanionLimitPatch.cs   # Removes companion limit
│           └── PartyLimitPatch.cs       # Removes clan party limit
└── tests/
    └── UnlimitedParty.Tests/
        ├── UnlimitedParty.Tests.csproj
        ├── CompanionLimitPatchTests.cs
        └── PartyLimitPatchTests.cs
```

## Build & Deploy

```powershell
cd UnlimitedParty
.\deploy.ps1
```

Or build manually:

```powershell
dotnet build src\UnlimitedParty\UnlimitedParty.csproj -c Release
```

## Running Tests

```powershell
dotnet test tests\UnlimitedParty.Tests\UnlimitedParty.Tests.csproj
```

## Installation (Manual)

1. Build the project (`Release` configuration)
2. Copy the `Module\SubModule.xml` to `<Bannerlord>\Modules\UnlimitedParty\`
3. Copy `UnlimitedParty.dll` and `0Harmony.dll` from `src\UnlimitedParty\bin\Release\net472\` to `<Bannerlord>\Modules\UnlimitedParty\bin\Win64_Shipping_Client\`
4. Enable **Unlimited Party** in the Bannerlord launcher

## Compatibility

- Works with Story Mode and Sandbox campaigns
- Compatible with other mods that don't also patch `DefaultClanTierModel.GetCompanionLimit` or `GetPartyLimitForTier`
