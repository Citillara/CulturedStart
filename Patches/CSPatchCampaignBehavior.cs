using HarmonyLib;
using StoryMode;
using StoryMode.Behaviors;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace zCulturedStart.Patches
{
    public class CSPatchCampaignBehavior
    {
        [HarmonyPatch(typeof(TrainingFieldCampaignBehavior), "OnCharacterCreationIsOver")]
        public class CSPatchTrainingFieldCampaignBehavior
        {
            private static void Prefix(ref bool ___SkipTutorialMission)
            {
                ___SkipTutorialMission = true;
                StoryModeManager.Current.MainStoryLine.CompleteTutorialPhase(true);
            }
        }
        [HarmonyPatch(typeof(TutorialPhaseCampaignBehavior), "OnStoryModeTutorialEnded")]
        public class CSPatchTutorialPhaseCampaignBehavior
        {
            private static bool Prefix()
            {
                DisableHeroAction.Apply(StoryModeHeroes.ElderBrother);
                StoryModeHeroes.ElderBrother.Clan = CampaignData.NeutralFaction;
                return false;
            }
        }
    }
}
