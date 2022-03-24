using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(CharacterCreationGenericStageVM), "RefreshSelectedOptions")]
    public class CSPatchCharacterCreationStageVM
    {
        private static CharacterCreationGenericStageVM _characterCreationGenericStageVM;

        public static void Postfix(CharacterCreationGenericStageVM __instance) => _characterCreationGenericStageVM = __instance;

        public static void OnNextStage() => _characterCreationGenericStageVM.OnNextStage();
    }
}
