using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;

namespace zCulturedStart.Patches
{
    //This patch is to make kingdom be created after banner is done.
    [HarmonyPatch(typeof(BannerEditorVM), "ExecuteDone")]
    class CSPatchBannerVMDone
    {
        private static void Postfix()
        {
            if ((CSCharCreationOption.CSSelectOption == 7 || CSCharCreationOption.CSSelectOption == 8) && Clan.PlayerClan.Kingdom == null)
            {
                CSApplyChoices.CSCreateKingdom();
            }
        }
    }
}
