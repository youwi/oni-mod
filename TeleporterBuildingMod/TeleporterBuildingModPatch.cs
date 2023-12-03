using HarmonyLib;
using UnityEngine;

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
    //可拆可建

    [HarmonyPatch(typeof(Deconstructable), "OnCompleteWork")]
    public class Deconstructable_OnCompleteWork_Patch
    {
        //预判出错的代码.  
        //手动拆除.

        public static bool Prefix(Deconstructable __instance)
        {
            Building component = __instance.GetComponent<Building>();
            // WarpReceiverConfig ,它不是building 所以会出错.
            if (__instance.GetComponent<WarpPortal>() != null)
            {
                __instance.gameObject.DeleteObject();
                Debug.LogWarning("------>WarpPortal:OnCompleteWork:..");
                // SimCellOccupier component2 = __instance.GetComponent<SimCellOccupier>();
                return false;
            };
            if (__instance.GetComponent<WarpReceiver>() != null)
            {
                __instance.gameObject.DeleteObject();
                Debug.LogWarning("------>WarpReceiver:OnCompleteWork:..");
                return false;
            };
            // GameHashes
            if (component == null)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(WarpPortal), "Discover")]
    public class WarpPortal_GetTargetWorldID_Patch
    {
        //预判出错的代码.

        public static bool Prefix(WarpPortal __instance)
        {
            //  WarpPortal.GetTargetWorldID();
            SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
            WarpReceiver[] array = Object.FindObjectsOfType<WarpReceiver>();
            bool found = false;//没有找到接收器.
            foreach (WarpReceiver component in array)
            {
                if (component.GetMyWorldId() != __instance.GetMyWorldId())
                {
                    found = true;
                    return true;// component.GetMyWorldId();
                }
            }
            if (found)
            {
                return false;
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
    [HarmonyPatch(typeof(ConduitSecondaryOutput), nameof(ConduitSecondaryOutput.HasSecondaryConduitType))]

    public class ConduitSecondaryOutputPatch
    {
        //这里有报错bug,所以打了个补丁.
        public static bool Prefix(ConduitSecondaryOutput __instance)
        {

            if (__instance.portInfo == null)
            {
                __instance.portInfo = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));
                return true;
            }
            return false;
        }

    }
    [HarmonyPatch(typeof(WarpReceiverConfig), "OnSpawn")]
    public class WarpReceiverConfigDeconstructionPatch
    {
        public static void Postfix(GameObject inst)
        {
            inst.AddOrGet<Deconstructable>();
            PrimaryElement component = inst.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Lead);
            var obj = inst.GetComponent<Deconstructable>();
            if (obj != null)
            {
                obj.allowDeconstruction = true;
            }
            // inst.FindOrAddComponent<Deconstructable>();
        }
    }
    [HarmonyPatch(typeof(WarpPortalConfig), "OnSpawn")]
    public class WarpPortalConfigDeconstructionPatch
    {
        public static void Postfix(GameObject inst)
        {
            PrimaryElement component = inst.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Lead);
            inst.AddOrGet<Deconstructable>();
            var obj = inst.GetComponent<Deconstructable>();
            if (obj != null)
                obj.allowDeconstruction = true;
            var warpPortal= inst.GetComponent<WarpPortal>();
            // 直接设置为发现.绕过Discover
            if(warpPortal != null) {
                Traverse.Create(warpPortal).Field("discovered").SetValue(true);
            }
            
            //warpPortal.discovered = true;
        }
    }

    //添加建筑到菜单中
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]

    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPPORTALHACK.DESC", STRINGS.BUILDINGS.PREFABS.WARPPORTAL.DESC });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPPORTALHACK.EFFECT", "WarpPortal" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPPORTALHACK.NAME", STRINGS.BUILDINGS.PREFABS.WARPPORTAL.NAME });

            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPRECEIVERHACK.DESC", STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.DESC });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPRECEIVERHACK.EFFECT", "WarpReceiver" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.WARPRECEIVERHACK.NAME", STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.NAME });
            //ModUtil.AddBuildingToPlanScreen("Base", "TeleportalPad"); //传送台没什么用.
            ModUtil.AddBuildingToPlanScreen("Base", "WarpPortalHack"); //使用了模样
            ModUtil.AddBuildingToPlanScreen("Base", "WarpReceiverHack");//使用了模样

            //ModUtil.AddBuildingToPlanScreen("Base", "WarpReceiver");//原生不支持
            //ModUtil.AddBuildingToPlanScreen("Base", "TeleporterTransmitter");//可能的翻译
            //ModUtil.AddBuildingToPlanScreen("Base", "TeleporterReceiver");//可能的翻译
        }

    }

    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public static class BuildingComplete_OnSpawn_Patch
    {

        public static void Postfix(BuildingComplete __instance)
        {
            GameObject go = __instance.gameObject;
            if (__instance.name == "WarpPortalHack"
                || __instance.name == "WarpPortalHackComplete"
                || __instance.name == "WarpReceiverHackComplete" //这里和配置的模板名一样.
                || __instance.name == "WarpReceiverHack"

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
                replaceBuilding(__instance.name, cell);
                // SimMessages.ModifyCellWorldZone(cell, Sim.SpaceZoneID);
                // DestroyCellWithBackground(cell);

                go.DeleteObject(); // remove Natural Tile
            }
        }
        /**
         * 使用模板来替换建筑.模板名放在模板目录下.
         */
        public static void replaceBuilding(string templateName, int cell)
        {
            //TemplateCache.
            TemplateContainer template = TemplateCache.GetTemplate(templateName);
            TemplateLoader.Stamp(template, Grid.CellToPos(cell), delegate
            {
                global::Debug.Log("替换打印了...");
            });
        }

    }
}
