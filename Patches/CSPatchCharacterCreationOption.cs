using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace zCulturedStart.Patches
{
    [HarmonyPatch(typeof(CharacterCreationOption), "SetTextVariables")]
    public class CSPatchCharacterCreationOption
    {
        // Get the text to change and the attribute points to add.
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            List<CodeInstruction> codesAtIndex = new List<CodeInstruction>();
            List<CodeInstruction> codesToInsert = new List<CodeInstruction>();
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Stloc_0 && codes[i - 1].opcode != OpCodes.Ldsfld)
                {
                    codes[i - 2].opcode = OpCodes.Nop;
                    codes[i - 1].opcode = OpCodes.Nop;
                    codesAtIndex.Add(codes[i]);
                }
            }
            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg_1));
            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg, 5));
            codesToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CSPatchCharacterCreationOption), "RemoveZeroPointText", new Type[] { typeof(string), typeof(List<SkillObject>), typeof(int) })));
            foreach (CodeInstruction codeAtIndex in codesAtIndex)
            {
                codes.InsertRange(codes.IndexOf(codeAtIndex), codesToInsert);
            }
            return codes;
        }
        // Remove redundant text if the attribute points to add is 0.
        public static TextObject RemoveZeroPointText(string text, List<SkillObject> skills, int attributeLevelToAdd)
        {
            if (attributeLevelToAdd == 0)
            {
                if (skills.Count == 3)
                {
                    return new TextObject("{=CulturedStart49}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}, {SKILL_TWO} and {SKILL_THREE}{NEWLINE}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}");
                }
                else if (skills.Count == 2)
                {
                    return new TextObject("{=CulturedStart50}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE} and {SKILL_TWO}{NEWLINE}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}");
                }
                else if (skills.Count == 1)
                {
                    return new TextObject("{=CulturedStart51}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}{NEWLINE}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}");
                }
                else
                {
                    return new TextObject(null);
                }
            }
            else
            {
                return new TextObject(text);
            }
        }
    }
}
