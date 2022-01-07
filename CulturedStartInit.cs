using HarmonyLib;
using StoryMode.Behaviors.Quests.FirstPhase;
using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace zCulturedStart
{
    class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var BannerInvestigationQuest = typeof(BannerInvestigationQuest).GetMethod("InitializeNotablesToTalkList", BindingFlags.NonPublic | BindingFlags.Instance);
            var postfix = typeof(CSTalkWithNoblePatch).GetMethod("NoblePatch", BindingFlags.NonPublic | BindingFlags.Static);
            Harmony harmony = new Harmony("mod.bannerlord.CS");

            harmony.Patch(BannerInvestigationQuest, new HarmonyMethod(postfix));

            harmony.PatchAll();
        }
    }
}
