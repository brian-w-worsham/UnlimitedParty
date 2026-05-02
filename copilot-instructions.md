# Copilot Instructions

This project is a Bannerlord singleplayer Harmony mod targeting .NET Framework 4.7.2.

## Goals

- Remove the clan companion limit so the player can recruit as many
  companions as desired regardless of clan tier.
- Remove the clan party limit so the player can field as many parties as
  desired regardless of clan tier.
- Make the change save-compatible: enabling or disabling the mod must
  not corrupt an existing campaign.
- Keep the patch surface as small and focused as possible.

## Project Conventions

- Keep source in `src/UnlimitedParty/`.
- Keep tests in `tests/UnlimitedParty.Tests/`.
- Use `internal` for test-targeted helpers and expose internals via
  `InternalsVisibleTo` to `UnlimitedParty.Tests`.
- Keep patch classes in the `Patches` folder, one patch per file.
- Use the `UnlimitedParty.Patches` namespace for patch classes.
- Use the `UnlimitedParty` namespace for `SubModule` and shared logic.

## Patch Safety Rules

- Patch only `DefaultClanTierModel.GetCompanionLimit(Clan)` and
  `DefaultClanTierModel.GetPartyLimitForTier(Clan, int)`.
- Always use a Harmony **Prefix** that sets `__result = 9999` and
  returns `false` to skip the original method. Do not use a Postfix
  here — overriding the calculation entirely is the intended behaviour.
- Do not patch `Clan`, `MobileParty`, `CompanionRoles`, or any other
  class — keep the surface area minimal.
- Use `9999` as the high cap (not `int.MaxValue`) to avoid overflow in
  any vanilla code that adds to the returned value.
- Use a unique Harmony id (`com.unlimitedparty.bannerlord`) and unpatch
  with that same id in `OnSubModuleUnloaded`.

## Build and Test

- Build: `dotnet build src/UnlimitedParty/UnlimitedParty.csproj -c Release`
- Test: `dotnet test tests/UnlimitedParty.Tests/UnlimitedParty.Tests.csproj`
- Deploy: `./deploy.ps1`

## Deploy Expectations

- Deployment target: `<Bannerlord>/Modules/UnlimitedParty/`
- Required outputs:
  - `Module/SubModule.xml`
  - `bin/Win64_Shipping_Client/UnlimitedParty.dll`
  - `bin/Win64_Shipping_Client/0Harmony.dll`

## Testing Strategy

- Test each patch's `Prefix` directly in
  `CompanionLimitPatchTests.cs` and `PartyLimitPatchTests.cs`:
  - The `__result` ref parameter is always overwritten with `9999`.
  - The Prefix always returns `false` so the original method is
    skipped.
  - The behaviour holds for any starting `__result` (zero, low, high).
- Validate the Harmony attribute metadata on each patch class:
  - `[HarmonyPatch]` is present.
  - `declaringType` is `DefaultClanTierModel`.
  - `methodName` is exactly `GetCompanionLimit` /
    `GetPartyLimitForTier`.
  - `argumentTypes` matches the targeted overload.
- Aim for >80% coverage on the patch classes.
- Prefer reflection-based attribute tests over constructing real
  `Clan` or `DefaultClanTierModel` instances — those require live
  campaign state which is impractical in unit tests.

## Known Limitations

- If a future Bannerlord update renames `GetCompanionLimit` or
  `GetPartyLimitForTier`, the Harmony patches will silently fail to
  apply. Update the `[HarmonyPatch]` attribute and the test that
  validates argument types together.
- Other mods that also patch `DefaultClanTierModel` may conflict.
- The mod will be tested with Bannerlord v1.0+.

## Code Style

- Use C# 9.0 features where applicable.
- Add XML documentation comments to every public/internal class and
  member.
- Keep methods small and focused on a single responsibility.
- Use descriptive variable names; comment any non-obvious branching.
