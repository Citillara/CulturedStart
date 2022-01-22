using HarmonyLib;
using SandBox;
using StoryMode;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using zCulturedStart.Patches;

namespace zCulturedStart
{
    public class CulturedStartSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            _harmony = new Harmony("mod.bannerlord.culturedstart");
            _harmony.PatchAll();
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (CulturedStartSettings.Instance.ShouldSkipTWLogo)
            {
                AccessTools.Field(typeof(Module), "_splashScreenPlayed").SetValue(Module.CurrentModule, true);
            }
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarter)
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
