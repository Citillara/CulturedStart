using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(CharacterCreationState), "OnInitialize")]
    public class CSPatchCharacterCreationState
    {
        public static void Postfix(CharacterCreationState __instance)
        {
            if (CulturedStartSettings.Instance.ShouldSkipCharacterCreation)
            {
                CharacterCreationContentBase characterCreationContent = CharacterCreationContentBase.Instance;
                characterCreationContent.SetSelectedCulture(characterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance.CharacterCreation);
                __instance.FinalizeCharacterCreation();
            }
        }
    }
}
