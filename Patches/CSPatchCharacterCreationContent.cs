using HarmonyLib;
using StoryMode.CharacterCreationContent;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem.CharacterCreationContent;

namespace zCulturedStart.Patches
{
    public class CSPatchCharacterCreationContent
    {
        [HarmonyPatch]
        public class CSPatchCharacterCreationInitialized
        {
            public static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(SandboxCharacterCreationContent), "OnInitialized");
                yield return AccessTools.Method(typeof(StoryModeCharacterCreationContent), "OnInitialized");
            }
            protected static void Postfix(MethodBase __originalMethod, CharacterCreation characterCreation)
            {
                CulturedStartCharacterCreationContent characterCreationContent = new CulturedStartCharacterCreationContent();
                if (__originalMethod.DeclaringType == typeof(StoryModeCharacterCreationContent))
                {
                    characterCreationContent.AddQuestMenu(characterCreation);
                }
                characterCreationContent.AddStartMenu(characterCreation);
                characterCreationContent.AddLocationMenu(characterCreation);
            }
        }
        [HarmonyPatch]
        public class CSPatchCharacterCreationFinalized
        {
            public static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(SandboxCharacterCreationContent), "OnCharacterCreationFinalized");
                yield return AccessTools.Method(typeof(StoryModeCharacterCreationContent), "OnCharacterCreationFinalized");
            }
            public static void Postfix() => CulturedStartHelper.ApplyStartOptions();
        }
    }
}
