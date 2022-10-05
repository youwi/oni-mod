using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace TeleporterBuildingMod
{
    //复制人传送器有多个类：WarpPortal，Teleporter
/*    [HarmonyPatch(typeof(WarpPortal))]

    public class TeleporterBuildingModPatch
    {
        [HarmonyPatch("CreateBuildingDef")]
        public static BuildingDef Postfix(BuildingDef __result)
        {
            __result.ShowInBuildMenu = true;
            return __result;

        }

    }
    //添加建筑到菜单中
    [HarmonyPatch(typeof(Teleporter))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            ModUtil.AddBuildingToPlanScreen("Base", "Tele");
            ModUtil.AddBuildingToPlanScreen("Base", "WarpConduitReceiverConfig");
        }

    }*/
}
