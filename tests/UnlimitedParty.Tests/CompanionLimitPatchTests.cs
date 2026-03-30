using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using UnlimitedParty.Patches;
using Xunit;

namespace UnlimitedParty.Tests
{
    /// <summary>
    /// Tests for <see cref="CompanionLimitPatch"/> Harmony prefix method and attributes.
    /// </summary>
    public class CompanionLimitPatchTests
    {
        [Fact]
        public void Prefix_SetsResultTo9999()
        {
            int result = 3;
            bool executeOriginal = CompanionLimitPatch.Prefix(ref result);

            Assert.Equal(9999, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void Prefix_SetsResultTo9999_WhenAlreadyHigh()
        {
            int result = 100;
            bool executeOriginal = CompanionLimitPatch.Prefix(ref result);

            Assert.Equal(9999, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void Prefix_SetsResultTo9999_WhenZero()
        {
            int result = 0;
            bool executeOriginal = CompanionLimitPatch.Prefix(ref result);

            Assert.Equal(9999, result);
            Assert.False(executeOriginal);
        }

        [Fact]
        public void Patch_HasCorrectHarmonyPatchAttribute()
        {
            var attr = typeof(CompanionLimitPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void Patch_TargetsDefaultClanTierModel()
        {
            var attrs = typeof(CompanionLimitPatch).GetCustomAttributes<HarmonyPatch>();
            var attr = Assert.Single(attrs);
            Assert.Equal(typeof(DefaultClanTierModel), attr.info.declaringType);
        }

        [Fact]
        public void Patch_TargetsGetCompanionLimitMethod()
        {
            var attr = typeof(CompanionLimitPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.Equal("GetCompanionLimit", attr.info.methodName);
        }

        [Fact]
        public void Patch_SpecifiesCorrectArgumentTypes()
        {
            var attr = typeof(CompanionLimitPatch).GetCustomAttribute<HarmonyPatch>();
            Assert.NotNull(attr.info.argumentTypes);
            Assert.Single(attr.info.argumentTypes);
            Assert.Equal(typeof(Clan), attr.info.argumentTypes[0]);
        }
    }
}
