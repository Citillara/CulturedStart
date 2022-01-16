﻿using HarmonyLib;
using StoryMode.Behaviors.Quests.FirstPhase;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(BannerInvestigationQuest), "InitializeNotablesToTalkList")]
    public class CSPatchBannerInvestigationQuest
    {
        private static void Postfix(BannerInvestigationQuest __instance)
        {
            if (CulturedStartHelper.QuestOption == 1)
            {
                __instance.CompleteQuestWithSuccess();
            }
        }
    }
}
