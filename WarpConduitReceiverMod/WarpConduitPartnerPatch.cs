using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpConduitPartnerMod
{
    [HarmonyPatch(typeof(WarpConduitReceiverConfig))]

    public class WarpConduitPartnerPatch
    {
        [HarmonyPatch("CreateBuildingDef")]
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu=true;
            return __result;
            //WarpConduitPartner

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
    //添加建筑到菜单中
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitSenderConfig");
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitReceiverConfig");
        }

    }
}
