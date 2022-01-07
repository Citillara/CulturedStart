using HarmonyLib;
using StoryMode.CharacterCreationContent;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.ObjectSystem;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(StoryModeCharacterCreationContent), "OnInitialized")]
    class CSCharCreationPatch
    {
        private static void Prefix(CharacterCreation characterCreation)
        {
            //Change here to make addtl cultures first one to load
            int maincultures = 0;
            foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
            {
                if (cultureObject.IsMainCulture)
                {
                    maincultures++;
                }
            }
            if (maincultures == 6)
            {
                CultureStartOptions.AddGameOption(characterCreation);
            }
            else
            {
                CultureStartOptions.AddtlCultures(characterCreation);
                CultureStartOptions.AddGameOption(characterCreation);
            }
        }
    }
}
