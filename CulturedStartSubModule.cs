using HarmonyLib;
using StoryMode.GameComponents.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace zCulturedStart
{
    // This mod adds new character creation options to customize the game start. It is a fork of the original mod by Barhidous, which I took over after it was discontinued.
    public class CulturedStartSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            Harmony harmony = new Harmony("mod.bannerlord.culturedstart");
            harmony.PatchAll();
            harmony.Unpatch(AccessTools.Method(typeof(TrainingFieldCampaignBehavior), "OnCharacterCreationIsOver"), HarmonyPatchType.All, "mod.bannerlord.usefulskips");
            harmony.Unpatch(AccessTools.Method(typeof(TutorialPhaseCampaignBehavior), "OnStoryModeTutorialEnded"), HarmonyPatchType.All, "mod.bannerlord.usefulskips");
        }

        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            CulturedStartManager manager = CulturedStartManager.Current;
            manager.SetQuestOption(0);
            manager.SetStoryOption(0);
            manager.SetLocationOption(0);
        }
    }
}
