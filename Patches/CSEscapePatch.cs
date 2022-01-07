using HarmonyLib;
using StoryMode.CharacterCreationContent;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;

namespace zCulturedStart
{
    [HarmonyPatch(typeof(StoryModeCharacterCreationContent), "EscapeOnInit")]
    class CSEscapePatch
    {
        private static void Prefix(CharacterCreation characterCreation)
        {
            //Major change due to change in escape, i'm just patching on this function not modifying the menu anymore. Will show brother/siblings and shit but \o/
            List<CharacterCreationMenu> CurMenus = (List<CharacterCreationMenu>)AccessTools.Field(typeof(CharacterCreation), "CharacterCreationMenus").GetValue(characterCreation);
            bool loaded = false;
            foreach (CharacterCreationMenu x in CurMenus)
            {
                if (x.Text.ToString() == "Beginning your new adventure")
                {
                    loaded = true;
                    break;
                }
            }
            if (!loaded)
            {
                CultureStartOptions.AddStartLocation(characterCreation);
            }
        }
    }
}
