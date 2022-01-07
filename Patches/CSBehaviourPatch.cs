using HarmonyLib;
using StoryMode;
using TaleWorlds.CampaignSystem;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(StoryModeSubModule), "AddBehaviors")]
    class CSBehaviourPatch
    {
        private static void Postfix(CampaignGameStarter campaignGameStarter) => campaignGameStarter.AddBehavior(new CSBehaviour());
    }
}
