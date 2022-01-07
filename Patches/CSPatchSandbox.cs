using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "OnCharacterCreationFinalized")]
    class CSPatchSandbox
    {
        private static void Postfix()
        {
            CSApplyChoices.ApplyStoryOptions();
            if (CSCharCreationOption.CSLocationOption != 8 && CSCharCreationOption.CSLocationOption != 9)
            {
                Vec2 StartPos = CulturedStartLocPatch.GetSettlementLoc(CSCharCreationOption.CSOptionSettlement());
                MobileParty.MainParty.Position2D = StartPos;
                MapState mapstate = GameStateManager.Current.ActiveState as MapState;
                mapstate.Handler.TeleportCameraToMainParty();
            }
        }
    }
}
