using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delaunay.Geo;
using HarmonyLib;
using Klei;
using ProcGen;
using UnityEngine;
using static Klei.WorldDetailSave;

namespace VacuumSpaceMod
{


    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public static class BuildingComplete_OnSpawn_Patch
    {

        public static void Postfix(BuildingComplete __instance)
        {
            GameObject go = __instance.gameObject;
            if (__instance.name == "VacuumSpaceModComplete")
            {
                Vector3 pos = go.transform.position;
                PrimaryElement element = go.GetComponent<PrimaryElement>();
                int cell = Grid.PosToCell(pos);
                 

                //替换成石块
                //SimMessages.ReplaceAndDisplaceElement(cell, element.ElementID,null, 50f, element.Temperature, byte.MaxValue, 0, -1); // spawn Natural Block
                //猜太空背景为
                // replaceBuilding(__instance.name, cell);

                DebugViewClassPath.markCellToSpace(pos);
                go.DeleteObject(); // remove Natural Tile
            }
        }
        /**
         * 使用模板来替换建筑.模板名放在模板目录下.
         */
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
            var zoneRenderData = World.Instance.zoneRenderData;

            Console.WriteLine("zoneRenderData");
            if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
            {
                SimMessages.ReplaceElement(cell, SimHashes.Void, null, 0f, 0f, byte.MaxValue, 0, -1);
                return;
            }
            SimMessages.ReplaceElement(cell, SimHashes.Vacuum, null, 0f, 0f, byte.MaxValue, 0, -1);
        }
    }

    //OnActiveWorldChanged
    [HarmonyPatch(typeof(SubworldZoneRenderData), "OnActiveWorldChanged")]
    public class DebugViewClassPath
    {
        /*public static void Postfix()
        {
            Console.WriteLine("测试切换世界:OnActiveWorldChanged");
        }*/
        public static void Prefix()
        {
            Console.WriteLine("测试标记所有为太空:markCellToSpace");
            // markCellToSpace();
        }
        public static void markAllCellToSpace()
        {
            WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
            Vector2 zero = Vector2.zero;
            for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
            {
                WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
                Polygon poly = overworldCell.poly;

                //强制修改所有地为太空背景.  Space为7
                overworldCell.zoneType = SubWorld.ZoneType.Space;
            }
        }
        public static void markCellToSpace(Vector3 pos)
        {
            WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
            //World.Instance.wo;
            Vector2 zero = Vector2.zero;
            int currentCell = Grid.PosToCell(pos);

            for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
            {
                WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
                Polygon poly = overworldCell.poly;
                zero.y = (float)((int)Mathf.Floor(poly.bounds.yMin));
                while (zero.y < Mathf.Ceil(poly.bounds.yMax))
                {
                    zero.x = (float)((int)Mathf.Floor(poly.bounds.xMin));
                    while (zero.x < Mathf.Ceil(poly.bounds.xMax))
                    {
                        if (poly.Contains(zero))
                        {
                            int num = Grid.XYToCell((int)zero.x, (int)zero.y);
                            if (Grid.IsValidCell(num))
                            {
                                //遍历找区块,找到就标记为太空.
                                if (num==currentCell)
                                {
                                    Console.WriteLine("测试标记为太空:for/while : " + currentCell  );
                                    overworldCell.zoneType = SubWorld.ZoneType.Space;
                                    //分割区块.
                                    break;
                                }

                            }
                        }
                        zero.x += 1f;
                    }
                    zero.y += 1f;
                }
            }
            /*
            if (block <= clusterDetailSave.overworldCells.Count)
            {
                WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[block];
                //  Space为7
                overworldCell.zoneType = SubWorld.ZoneType.Space;
                Console.WriteLine("测试标记为太空:markCellToSpace:" + cell+"/"+ clusterDetailSave.overworldCells.Count);

            }*/
        }

    }

    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public class GeneratedBuildingsPatch
    {
        private static void Prefix()
        {
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.VACUUMSPACEMOD.DESC", "make bomb destory all" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.VACUUMSPACEMOD.EFFECT", "make bomb destory all" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.VACUUMSPACEMOD.NAME", "SpaceBomb" });


            ModUtil.AddBuildingToPlanScreen("Base", "VacuumSpaceMod");
        }
    }
}