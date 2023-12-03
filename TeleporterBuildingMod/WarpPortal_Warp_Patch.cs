using HarmonyLib;

namespace TeleporterBuildingMod
{
    [HarmonyPatch(typeof(WarpPortal), "Warp")]
    public class WarpPortal_Warp_Patch
    {
        public static bool Prefix(WarpPortal __instance)
        {

            return true;
        }
        public static void WarpNew(WarpPortal __instance)
        {
            if (__instance.worker == null || __instance.worker.HasTag(GameTags.Dying) || __instance.worker.HasTag(GameTags.Dead))
            {
                return;
            }

            WarpReceiver warpReceiver = null;
            WarpReceiver[] array = UnityEngine.Object.FindObjectsOfType<WarpReceiver>();
            foreach (WarpReceiver warpReceiver2 in array)
            {
                if (warpReceiver2.GetMyWorldId() != __instance.GetMyWorldId())
                {
                    warpReceiver = warpReceiver2;
                    break;
                }
            }

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
