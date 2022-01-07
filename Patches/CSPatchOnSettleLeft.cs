using HarmonyLib;
using StoryMode.Behaviors;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(FirstPhaseCampaignBehavior), "OnSettlementLeft")]
    class CSPatchOnSettleLeft
    {
        private static bool Prefix() => false; //Not letting anything happen here, because I remove listeners in CSPatchDisableTutorial
    }
}
