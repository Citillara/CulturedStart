using HarmonyLib;
using StoryMode;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(MainStoryLine), "CompleteTutorialPhase")]
    class CSPatchDisableTutorial
    {
        private static bool Prefix(MainStoryLine __instance)
        {
            if (CSCharCreationOption.CSGameOption == 2)
            {
                TutorialPhase.Instance.CompleteTutorial(true);
                CSOnStoryModeEnded();
                __instance.GetType().GetProperty("FirstPhase").SetValue(__instance, new FirstPhase());
                return false;
            }
            return true;
        }

        private static void CSOnStoryModeEnded()
        {
            foreach (MobileParty tracked in MobileParty.All)
            {
                Campaign.Current.VisualTrackerManager.RemoveTrackedObject(tracked);
            }
        }
    }
}
