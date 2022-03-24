using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(CharacterCreationState))]
    public class CSPatchCharacterCreationState
    {
        // Set a random culture.
        [HarmonyPostfix]
        [HarmonyPatch("OnInitialize")]
        public static void Postfix1(CharacterCreationState __instance)
        {
            if (CulturedStartSettings.Instance.MenusToSkip.SelectedIndex > 0)
            {
                CharacterCreationContentBase characterCreationContent = CharacterCreationContentBase.Instance;
                characterCreationContent.SetSelectedCulture(characterCreationContent.GetCultures().GetRandomElementInefficiently(), __instance.CharacterCreation);
            }
        }

        // Skip character creation.
        [HarmonyPostfix]
        [HarmonyPatch("NextStage")]
        public static void Postfix2(CharacterCreationState __instance)
        {
            if (CulturedStartSettings.Instance.MenusToSkip.SelectedIndex == 1)
            {
                if (__instance.GetIndexOfCurrentStage() < 2)
                {
                    __instance.NextStage();
                }
                else if (__instance.GetIndexOfCurrentStage() == 2)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        CSPatchCharacterCreationStageVM.OnNextStage();
                    }
                }
                else
                {
                    __instance.FinalizeCharacterCreation();
                }
            }
            else if (CulturedStartSettings.Instance.MenusToSkip.SelectedIndex == 2)
            {
                __instance.FinalizeCharacterCreation();
            }
        }
    }
}
