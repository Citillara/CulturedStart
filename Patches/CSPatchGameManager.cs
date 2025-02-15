﻿using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace zCulturedStart.Patches
{
    public class CSPatchGameManager
    {
        // Remove the instructions which play the campaign intro.
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            List<CodeInstruction> codesToInsert = new List<CodeInstruction>();
            CodeInstruction codeAtIndex = null;
            MethodInfo method = null;
            int startIndex = 0;
            int endIndex = 0;
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Stloc_0)
                {
                    startIndex = i - 3;
                }
                if (codes[i].opcode == OpCodes.Ldloc_0)
                {
                    endIndex = i + 2;
                    codeAtIndex = codes[i + 3];
                    method = (MethodInfo)codes[i - 5].operand;
                }
            }
            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg_0));
            codesToInsert.Add(new CodeInstruction(OpCodes.Call, method));
            codes.RemoveRange(startIndex, endIndex - startIndex + 1);
            codes.InsertRange(codes.IndexOf(codeAtIndex), codesToInsert);
            return codes;
        }
    }
}
