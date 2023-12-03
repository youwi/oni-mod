using HarmonyLib;
using UnityEngine;

namespace WarpConduitPartnerMod
{
    [HarmonyPatch(typeof(WarpPortal), "Discover")]
    public class WarpPortal_GetTargetWorldID_Patch
    {
        //预判出错的代码.
       
        public static bool Prefix(WarpPortal __instance)
        {
            //  WarpPortal.GetTargetWorldID();
            SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
            WarpReceiver[] array = Object.FindObjectsOfType<WarpReceiver>();
            foreach (WarpReceiver component in array)
            {
                if (component.GetMyWorldId() != __instance.GetMyWorldId())
                {
                    return true;// component.GetMyWorldId();
                }
            }
            if (array.Length == 1) //仅有一个,并且不是当前星的.
            {
                //没有就不要了.
                return false;
            }
            else
            {
                return true;
            }
                
        }
    }
    [HarmonyPatch(typeof(WarpConduitReceiverConfig), "DoPostConfigureComplete")]
    public class DeconstructionReceiverPatch
    {
        //添加可拆 接口器
        public static void Postfix(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
        }
    }
    [HarmonyPatch(typeof(WarpConduitSenderConfig), "DoPostConfigureComplete")]
    public class DeconstructionSenderPatch
    {
        //添加可拆 发送器

        public static void Postfix(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
        }
    }


    //添加2建筑到菜单中
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]

    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitSender");
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitReceiver");
        }

    }
    [HarmonyPatch(typeof(WarpConduitReceiverConfig), "CreateBuildingDef")]
    public class WarpConduitPartnerDefPatch
    {
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;
        }

    }
    [HarmonyPatch(typeof(WarpConduitSenderConfig), "CreateBuildingDef")]
    public class WarpConduitSenderConfigDefPatch
    {
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;

        }

    }
}
