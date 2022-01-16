using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace zCulturedStart
{
    public class CulturedStartSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad() => new Harmony("mod.bannerlord.CS").PatchAll();
    }
}
