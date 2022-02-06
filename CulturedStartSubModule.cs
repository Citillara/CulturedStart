using HarmonyLib;
using SandBox;
using StoryMode;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using zCulturedStart.Patches;

namespace zCulturedStart
{
    // This mod adds new character creation options to customize the game start. It is a fork of the original mod by Barhidous, which I took over after it was discontinued.
    public class CulturedStartSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            _harmony = new Harmony("mod.bannerlord.culturedstart");
            _harmony.PatchAll();
        }
        // Skip the TW logo.
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (CulturedStartSettings.Instance.ShouldSkipTWLogo)
            {
                AccessTools.Field(typeof(Module), "_splashScreenPlayed").SetValue(Module.CurrentModule, true);
            }
        }
        // Skip the campaign intro.
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            if (CulturedStartSettings.Instance.ShouldSkipCampaignIntro)
            {
                _harmony.Patch(AccessTools.Method(typeof(SandBoxGameManager), "OnLoadFinished"), transpiler: new HarmonyMethod(AccessTools.Method(typeof(CSPatchGameManager), "Transpiler")));
                _harmony.Patch(AccessTools.Method(typeof(StoryModeGameManager), "OnLoadFinished"), transpiler: new HarmonyMethod(AccessTools.Method(typeof(CSPatchGameManager), "Transpiler")));
            }
        }
        public override void OnGameEnd(Game game)
        {
            _harmony.Unpatch(AccessTools.Method(typeof(SandBoxGameManager), "OnLoadFinished"), AccessTools.Method(typeof(CSPatchGameManager), "Transpiler"));
            _harmony.Unpatch(AccessTools.Method(typeof(StoryModeGameManager), "OnLoadFinished"), AccessTools.Method(typeof(CSPatchGameManager), "Transpiler"));
        }
        private Harmony _harmony;
    }
}
