using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(CharacterCreationGenericStageVM), "RefreshSelectedOptions")]
    public class CSPatchCharacterCreationStageVM
    {
        public static void Postfix(CharacterCreationGenericStageVM __instance) => _characterCreationGenericStageVM = __instance;
        public static void OnNextStage() => _characterCreationGenericStageVM.OnNextStage();
        private static CharacterCreationGenericStageVM _characterCreationGenericStageVM;
    }
}
