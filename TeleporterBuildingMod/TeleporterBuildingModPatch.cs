using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using HarmonyLib;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using static STRINGS.UI.CLUSTERMAP;

namespace TeleporterBuildingMod
{
    //复制人传送器有多个类：WarpPortal,WarpReceiver，Teleporter,TeleportalPad
    //分不明白这几个类..
    //传送发射器.Teleporter Transmitter ,WarpPortal
    //传送接收器 Teleporter Receiver ,WarpReceiver

    //传送台(很旧的东西) TeleportalPad
    /*    [HarmonyPatch(typeof(TeleportalPadConfig))]

        public class DeconstructionPatch2
        {
            [HarmonyPatch("DoPostConfigureComplete")]
            public static void  Postfix(GameObject go)
            {
                go.GetComponent<Deconstructable>().allowDeconstruction = true;
            }   //TeleporterBuildingMod.DeconstructionPatch2::Postfix

        }*/
    /*    [HarmonyPatch(typeof(WarpPortalConfig))]
        public class WarpPortalConfigShowPatch
        {
            [HarmonyPatch("CreateBuildingDef")]
            public static BuildingDef Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = true;
                return __result;

            }

        }*/
    /*    //可拆可建
        [HarmonyPatch(typeof(WarpPortalConfig))]
        public class WarpPortalConfigDeconstructionPatch
        {
            [HarmonyPatch("OnPrefabInit")]
            public static void Postfix(GameObject __instance)
            {
                __instance.GetComponent<Deconstructable>().allowDeconstruction = true;
            }
        }
        [HarmonyPatch(typeof(WarpReceiverConfig))]
        public class WarpReceiverConfigDeconstructionPatch
        {
            [HarmonyPatch("OnPrefabInit")]
            public static void Postfix(GameObject __instance)
            {
                __instance.GetComponent<Deconstructable>().allowDeconstruction = true;
            }
        }*/
    //添加建筑到菜单中
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            //    ModUtil.AddBuildingToPlanScreen("Base", "TeleportalPad"); //传送台没什么用.
            ModUtil.AddBuildingToPlanScreen("Base", "WarpPortal1");
            ModUtil.AddBuildingToPlanScreen("Base", "WarpReceiver1");

            //    ModUtil.AddBuildingToPlanScreen("Base", "WarpReceiver");//原生不支持
            //    ModUtil.AddBuildingToPlanScreen("Base", "TeleporterTransmitter");//可能的翻译
            //    ModUtil.AddBuildingToPlanScreen("Base", "TeleporterReceiver");//可能的翻译
        }

    }

    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public static class BuildingComplete_OnSpawn_Patch
    {

        public static void Postfix(BuildingComplete __instance)
        {
            GameObject go = __instance.gameObject;
            if (__instance.name == "WarpPortal1"
                || __instance.name == "WarpPortal1Complete"
                || __instance.name == "WarpReceiver1Complete" //这里和配置的模板名一样.
                || __instance.name == "WarpReceiver1"

                )
            {
                Vector3 pos = go.transform.position;
                PrimaryElement element = go.GetComponent<PrimaryElement>();
                float temperature = element.Temperature;
                float mass = 50f;
                int cell = Grid.PosToCell(pos);

                //替换成石块
                SimMessages.ReplaceAndDisplaceElement(cell, element.ElementID,
                    null, mass, temperature, byte.MaxValue, 0, -1); // spawn Natural Block
                //猜太空背景为
                replaceBuilding(__instance.name,cell);
                // SimMessages.ModifyCellWorldZone(cell, Sim.SpaceZoneID);
                // DestroyCellWithBackground(cell);

                go.DeleteObject(); // remove Natural Tile
            }
        }
        public static void replaceBuilding(string templateName, int cell)
        {
            TemplateContainer template = TemplateCache.GetTemplate(templateName);
            TemplateLoader.Stamp(template, Grid.CellToPos(cell), delegate
            {
                Console.WriteLine("替换打印了...");
            });
        }
        public static void DestroyCellWithBackground(int cell)
        {
            foreach (GameObject gameObject in new List<GameObject>
        {
            Grid.Objects[cell, 2],
            Grid.Objects[cell, 1],
            Grid.Objects[cell, 12],
            Grid.Objects[cell, 16],
            Grid.Objects[cell, 0],
            Grid.Objects[cell, 26],
            Grid.Objects[cell,29] 
            // Grid.SceneLayer.Background; =-1 //这个溢出
            //InteriorWall  16
            //BackWall  1
            //Ground = 29,
        })
            {
                if (gameObject != null)
                {
                    UnityEngine.Object.Destroy(gameObject);
                    /*    World.Instance.groundRenderer;
                        SaveGame.Instance.get;
                        Grid.Spawnable;*/
                }
            }
            // World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
            // World.Instance.zoneRenderData;
            if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
            {
                SimMessages.ReplaceElement(cell, SimHashes.Void, null, 0f, 0f, byte.MaxValue, 0, -1);
                return;
            }
            SimMessages.ReplaceElement(cell, SimHashes.Vacuum, null, 0f, 0f, byte.MaxValue, 0, -1);
        }
    }
}
