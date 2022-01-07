using StoryMode.Behaviors.Quests.FirstPhase;
using System.Reflection;

namespace zCulturedStart
{
    class CSTalkWithNoblePatch
    {
        private static void NoblePatch(BannerInvestigationQuest __instance)
        {
            if (CSCharCreationOption.CSGameOption == 1)
            {
                __instance.GetType().GetField("_allNoblesDead", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, true);
            }
        }
    }
}
