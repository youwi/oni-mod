using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuitLocker;

namespace DeleteFreeChoreMod
{
    [HarmonyPatch(typeof(ReturnSuitWorkable), nameof(ReturnSuitWorkable.CreateChore))]
    public class DeleteChorePatch
    {
         public static void Postfix(ReturnSuitWorkable __instance)
        {
            if (__instance != null)
            {
               var ss= Traverse.Create(__instance)
                    .Field("idleChore")
                    .GetValue<WorkChore<SuitLocker.ReturnSuitWorkable>>();
                ss.Cancel("ReturnSuitWorkable.CancelChore");
            }
            Debug.LogWarning("取消任务：ReturnSuitWorkable CancelChore---->>>");
        }

    }
}
