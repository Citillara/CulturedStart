using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(BannerEditorVM), "SetClanRelatedRules")]
    class CSPatchColorCreator
    {
        static void Prefix(ref bool canChangeBackgroundColor) => canChangeBackgroundColor = Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Leader == Hero.MainHero;
    }
}
