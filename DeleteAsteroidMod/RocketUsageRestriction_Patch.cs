using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeleteAsteroidMod
{
  // [HarmonyPatch(typeof(RocketUsageRestriction), "InitializeStates")]
   public class RockPatch2
    {
        public static bool Prefix(StateMachine.BaseState default_state, RocketUsageRestriction __instance)
        {
            if(__instance.root ==null)
                return false;
            return true;
        }
    }
    //[HarmonyPatch]
    public  class RocketUsageRestriction_Patch
    {

        //{ldftn System.Void <>c::<InitializeStates>b__6_0(StatesInstance smi)}
        public static MethodInfo TargetMethod()
            {
                return AccessTools.Method(
                    "RocketUsageRestriction/<>c:<InitializeStates>b__6_0",
                    new Type[] { typeof(StateMachine.BaseState) },
                    null);
            }
        public static bool Prefix(StateMachine.BaseState default_state )
        {
            if(default_state==null)
                return false;
            return true;
        }
        public static  IEnumerable<CodeInstruction> _______Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                for (int i = 1; i < codes.Count; i++)
                {
                    if (codes[i].OperandIs(typeof(SaveManager).GetMethod("Save")))
                    {
                        codes[i - 1].opcode = OpCodes.Ldc_I4_0;//using System.Reflection.Emit;
                    }
                }
                return codes.AsEnumerable<CodeInstruction>();
            }
 
    }
}
