using HarmonyLib;
using UnityEngine;

namespace WarpConduitPartnerMod
{
    [HarmonyPatch(typeof(WarpConduitReceiverConfig))]
    public class DeconstructionPatch
    {
        //添加可拆
        [HarmonyPatch("DoPostConfigureComplete")]
        public static void Postfix(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
        }
    }
    [HarmonyPatch(typeof(WarpConduitSenderConfig))]
    public class DeconstructionPatch2
    {
        //添加可拆
        [HarmonyPatch("DoPostConfigureComplete")]
        public static void Postfix(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;
        }
    }


    //添加建筑到菜单中
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitSender");
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitReceiver");
        }

    }
    [HarmonyPatch(typeof(WarpConduitReceiverConfig))]
    public class WarpConduitPartnerPatch
    {
        [HarmonyPatch("CreateBuildingDef")]
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;
        }

    }
    [HarmonyPatch(typeof(WarpConduitSenderConfig))]
    public class WarpConduitSenderConfigPatch
    {
        [HarmonyPatch("CreateBuildingDef")]
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;

        }

    }
}
