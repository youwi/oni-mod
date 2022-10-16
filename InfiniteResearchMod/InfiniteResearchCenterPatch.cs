 
using Database;
using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using static LogicGateBase;

namespace InfiniteResearch
{
    class InfiniteResearchCenterPatch : InfiniteWorkable
    {
        static bool IsEndlessWorking(Workable instance) => instance.GetComponent<InfiniteModeToggleButton>().isInfiniteMode;

        [HarmonyPatch(typeof(ResearchCenter), "UpdateWorkingState")]
        static class InfiniteResearch_Patch
        {
            public static void Postfix(ResearchCenter __instance, Chore ___chore)
            {
                AccessTools.Method(typeof(InfiniteResearchCenterPatch), nameof(InfiniteResearchCenterPatch.IsEndlessWorking));
                Console.WriteLine("UpdateWorkingState_patch: "+IsEndlessWorking(__instance));
                // var button=__instance.GetComponent<InfiniteResearchCenterButton>();
                // button.isInfiniteMode=true;
              
              

            }

            /*  static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                int counter = 1;
                

                              foreach (CodeInstruction i in instructions)
                                {
                                    if (counter <= 2 && i.OpCodeIs(OpCodes.Ldc_I4_0))
                                    {
                                        counter++;
                                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(InfiniteResearchCenter), nameof(InfiniteResearchCenter.IsEndlessWorking)));
                                    }
                                    else
                                        yield return i;
                                }*/

        
    }

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.GetPercentComplete))]
        static class ProgressBar_Patch
        {
            static bool Prefix(ResearchCenter __instance, ref float __result)
            {
                if (IsEndlessWorking(__instance) && __instance.worker != null)
                {
               
                    var fat = __instance.worker.GetComponent<AttributeLevels>();
                    if (fat != null)
                    {
                        __result =fat.GetPercentComplete("Learning");
                    }
                    
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(DuplicantStatusItems), "CreateStatusItems")]
        static class LearningStatusItem_Patch
        {
            static void Postfix(StatusItem ___Researching)
            {
                ___Researching.resolveStringCallback = (string str, object data) => getString(str, data, DUPLICANTS.STATUSITEMS.LEARNING.NAME);
                ___Researching.resolveTooltipCallback = (string str, object data) => getString(str, data, DUPLICANTS.STATUSITEMS.LEARNING.TOOLTIP);

                string getString(string str, object data, string nameOrTooltip)
                {
                    TechInstance tech_instance = Research.Instance.GetActiveResearch();
                    string result;
                    if (tech_instance != null)
                        result = str.Replace("{Tech}", tech_instance.tech.Name);
                    else
                        result = nameOrTooltip;
                    return result;
                }
            }
        }

        // Not necessary for functionality, but prevents the log from spamming warnings about adding research points with no target.
        [HarmonyPatch(typeof(ResearchCenter), "ConvertMassToResearchPoints")]
        static class PreventResearchPoints_Patch { static bool Prefix(ResearchCenter __instance) => !IsEndlessWorking(__instance); }

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.Sim200ms))]
        static class Sim_Patch
        {
            static void Postfix(ResearchCenter __instance, Chore ___chore)
            {
               
                RemoveDupe(__instance, ___chore, IsEndlessWorking);
            }
        }

        [HarmonyPatch(typeof(ResearchCenter), "CreateChore")]
        static class CreateChore_Patch { static void Postfix(ResearchCenter __instance, Chore __result)
            {
                Console.WriteLine("CreateChore ...ModifyChore");
              //  ModifyChore(__instance, __result, IsEndlessWorking);
            } 
        }

        /*           public Func<Chore.Precondition.Context, bool> GetFunc()
                   {
                       return new Func<Chore.Precondition.Context, bool>(IsEndlessWorking);
                   }


           public Chore CreateChore(object __instance, Func<Chore.Precondition.Context, bool> func)
           {
               return new WorkChore<ResearchCenter>(Db.Get().ChoreTypes.Research, __instance, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
               {
                   preemption_cb = func
               };
           }*/

    }

}
