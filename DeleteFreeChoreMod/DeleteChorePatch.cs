using HarmonyLib;
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
                var ss = Traverse.Create(__instance)
                     .Field("idleChore")
                     .GetValue<WorkChore<SuitLocker.ReturnSuitWorkable>>();
                if (ss != null)
                {
                    ss.Cancel("ReturnSuitWorkable.CancelChore");
                }

            }
            //Debug.LogWarning("取消任务：ReturnSuitWorkable CancelChore---->>>");
        }

    }
}
