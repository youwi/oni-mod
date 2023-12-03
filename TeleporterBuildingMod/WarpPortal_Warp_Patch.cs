using HarmonyLib;

namespace TeleporterBuildingMod
{
    [HarmonyPatch(typeof(WarpPortal), "Warp")]
    public class WarpPortal_Warp_Patch
    {
        public static bool Prefix(WarpPortal __instance)
        {
            WarpNew(__instance);
            return false;
        }
        public static void WarpNew(WarpPortal __instance)
        {
            if (__instance.worker == null || __instance.worker.HasTag(GameTags.Dying) || __instance.worker.HasTag(GameTags.Dead))
            {
                return;
            }

            WarpReceiver warpReceiver = null;
            WarpReceiver[] array = UnityEngine.Object.FindObjectsOfType<WarpReceiver>();

            var cid = __instance.GetMyWorldId();
            string recStr = "";
            WarpReceiver minReceiver = null;
            if (array.Length > 0)
                minReceiver = array[0];
            //找最小的
            foreach (WarpReceiver tmp in array)
            {
                if (minReceiver.GetMyWorldId() > tmp.GetMyWorldId())
                {
                    minReceiver = tmp;
                };
                recStr += tmp.GetMyWorldId() + ",";
            }
            //找下一个
            foreach (WarpReceiver tmp in array)
            {
                if (tmp.GetMyWorldId() == cid + 1)  //最大数量 ClusterManager.Instance.worldCount;
                {
                    warpReceiver = tmp;//  实际是要找最近一个ID
                    break;
                }
            }
            //如果没有下一个,给最小ID的一个.
            if (warpReceiver == null)
            {
                warpReceiver = minReceiver;
            }

            if (warpReceiver != null)
            {
                Debug.Log($"---->发送器world:{cid} 接收器worlds:{recStr} 选中:{warpReceiver.GetMyWorldId()}<---");
            }
            else
            {
                Debug.LogWarning($"---->发送器world:{cid} 接收器worlds:{recStr} 选中:未找到<---");
            }
            //如果还是为空,找全局的
            if (warpReceiver == null)
            {
                SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
                warpReceiver = UnityEngine.Object.FindObjectOfType<WarpReceiver>();
            }

            if (warpReceiver != null)
            {
                //  __instance.delayWarpRoutine = 
                __instance.StartCoroutine(__instance.DelayedWarp(warpReceiver));
            }
            else
            {
                Debug.LogWarning("No warp receiver found - maybe POI stomping or failure to spawn?");
            }

            if (SelectTool.Instance.selected == __instance.GetComponent<KSelectable>())
            {
                SelectTool.Instance.Select(null, skipSound: true);
            }
        }
    }
}
