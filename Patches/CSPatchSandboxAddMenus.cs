using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.ObjectSystem;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "StartingAgeOnInit")]
    class CSPatchSandboxAddMenus
    {
        //Duplicating for sandbox start
        private static void Prefix(CharacterCreation characterCreation)
        {
            //Change here to make addtl cultures first one to load
            int maincultures = 0;
            CSCharCreationOption.CSSandboxToggle = 1;
            foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
            {
                if (cultureObject.IsMainCulture)
                {
                    maincultures++;
                }
            }
            if (maincultures == 6)
            {
                CultureStartOptions.AddStartOption(characterCreation);
                CultureStartOptions.AddStartLocation(characterCreation);
            }
            else
            {
                CultureStartOptions.AddtlCultures(characterCreation);
                CultureStartOptions.AddStartOption(characterCreation);
                CultureStartOptions.AddStartLocation(characterCreation);
            }
        }
    }
}
