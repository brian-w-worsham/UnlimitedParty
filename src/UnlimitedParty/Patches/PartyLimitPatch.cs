using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace UnlimitedParty.Patches
{
    /// <summary>
    /// Patches <see cref="DefaultClanTierModel.GetPartyLimitForTier"/>
    /// to always return a very high value, effectively removing the clan party limit.
    /// </summary>
    [HarmonyPatch(typeof(DefaultClanTierModel), "GetPartyLimitForTier",
        new[] { typeof(Clan), typeof(int) })]
    internal static class PartyLimitPatch
    {
        /// <summary>
        /// Replaces the original method: always sets <paramref name="__result"/>
        /// to 9999 to allow virtually unlimited clan parties.
        /// </summary>
        /// <param name="__result">The return value of the original method.</param>
        /// <returns><c>false</c> to skip the original method.</returns>
        internal static bool Prefix(ref int __result)
        {
            __result = 9999;
            return false;
        }
    }
}
