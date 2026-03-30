using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using UnlimitedParty.Patches;
using Xunit;

namespace UnlimitedParty.Tests
{
    /// <summary>
    /// Tests for <see cref="PartyLimitPatch"/> Harmony prefix method and attributes.
    /// </summary>
    public class PartyLimitPatchTests
    {
        [Fact]
        public void Prefix_SetsResultTo9999()
        {
            int result = 2;
            bool executeOriginal = PartyLimitPatch.Prefix(ref result);

            Assert.Equal(9999, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void Prefix_SetsResultTo9999_WhenAlreadyHigh()
        {
            int result = 50;
            bool executeOriginal = PartyLimitPatch.Prefix(ref result);

            Assert.Equal(9999, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void Prefix_SetsResultTo9999_WhenZero()
        {
            int result = 0;
            bool executeOriginal = PartyLimitPatch.Prefix(ref result);

            Assert.Equal(9999, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void Patch_HasCorrectHarmonyPatchAttribute()
        {
            var attr = typeof(PartyLimitPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void Patch_TargetsDefaultClanTierModel()
        {
            var attrs = typeof(PartyLimitPatch).GetCustomAttributes<HarmonyPatch>();
            var attr = Assert.Single(attrs);
            Assert.Equal(typeof(DefaultClanTierModel), attr.info.declaringType);
        }

        [Fact]
        public void Patch_TargetsGetPartyLimitForTierMethod()
        {
            var attr = typeof(PartyLimitPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal("GetPartyLimitForTier", attr.info.methodName);
        }

        [Fact]
        public void Patch_SpecifiesCorrectArgumentTypes()
        {
            var attr = typeof(PartyLimitPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr.info.argumentTypes);
            Assert.Equal(2, attr.info.argumentTypes.Length);
            Assert.Equal(typeof(Clan), attr.info.argumentTypes[0]);
            Assert.Equal(typeof(int), attr.info.argumentTypes[1]);
        }
    }
}
