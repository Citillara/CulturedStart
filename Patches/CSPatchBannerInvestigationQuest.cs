using HarmonyLib;
using StoryMode.Behaviors.Quests.FirstPhase;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(BannerInvestigationQuest), "OnStartQuest")]
    public class CSPatchBannerInvestigationQuest
    {
        // Skip the first quest "Investigate Neretzes' Folly".
        private static void Postfix(BannerInvestigationQuest __instance)
        {
            if (CulturedStartHelper.QuestOption == 1)
            {
                __instance.CompleteQuestWithSuccess();
            }
        }
    }
}
