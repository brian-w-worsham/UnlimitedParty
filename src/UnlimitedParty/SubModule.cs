using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace UnlimitedParty
{
    /// <summary>
    /// Entry point for the UnlimitedParty mod. Applies Harmony patches on load
    /// to remove companion and clan party limits.
    /// </summary>
    public class SubModule : MBSubModuleBase
    {
        /// <summary>Harmony instance used to apply and revert all patches.</summary>
        private Harmony _harmony;

        /// <summary>
        /// Called when the module is first loaded. Applies all Harmony patches
        /// and displays a confirmation message.
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                _harmony = new Harmony("com.unlimitedparty.bannerlord");
                _harmony.PatchAll();
                InformationManager.DisplayMessage(
                    new InformationMessage("Unlimited Party: Loaded — no companion or party limits!", Colors.Green));
            }
            catch (System.Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"Unlimited Party load error: {ex.Message}", Colors.Red));
            }
        }

        /// <summary>
        /// Called when the module is unloaded. Reverts all Harmony patches
        /// applied by this mod.
        /// </summary>
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            _harmony?.UnpatchAll("com.unlimitedparty.bannerlord");
        }
    }
}
