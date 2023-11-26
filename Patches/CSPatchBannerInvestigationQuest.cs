using HarmonyLib;
using StoryMode.Quests.FirstPhase;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(BannerInvestigationQuest), "OnStartQuest")]
    public class CSPatchBannerInvestigationQuest
    {
        // Skip the first quest "Investigate Neretzes' Folly".
        private static void Postfix(BannerInvestigationQuest __instance)
        {
            if (CulturedStartManager.Current.SkipNeretzesFollyQuest)
            {
                __instance.CompleteQuestWithSuccess();
            }
        }
    }
}
