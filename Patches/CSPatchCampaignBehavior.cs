using HarmonyLib;
using StoryMode;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace zCulturedStart.Patches
{
    public class CSPatchCampaignBehavior
    {
        [HarmonyPatch(typeof(TrainingFieldCampaignBehavior), "OnCharacterCreationIsOver")]
        public class CSPatchTrainingFieldCampaignBehavior
        {
            // Skip the tutorial.
            private static void Prefix(ref bool ___SkipTutorialMission)
            {
                ___SkipTutorialMission = true;
                TutorialPhase.Instance.PlayerTalkedWithBrotherForTheFirstTime();
                StoryModeManager.Current.MainStoryLine.CompleteTutorialPhase(true);
            }
        }

        [HarmonyPatch(typeof(TutorialPhaseCampaignBehavior), "OnStoryModeTutorialEnded")]
        public class CSPatchTutorialPhaseCampaignBehavior
        {
            // Skip the vanilla code that sets the player's items and gold.
            private static bool Prefix()
            {
                DisableHeroAction.Apply(StoryModeHeroes.ElderBrother);
                StoryModeHeroes.ElderBrother.Clan = null;
                return false;
            }
        }
    }
}
