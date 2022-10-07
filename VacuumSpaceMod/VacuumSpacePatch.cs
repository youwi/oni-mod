using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delaunay.Geo;
using HarmonyLib;
using Klei;
using ProcGen;
using UnityEngine;
using static Klei.WorldDetailSave;
using static ProcGen.SubWorld;
using static STRINGS.UI.CLUSTERMAP;

namespace VacuumSpaceMod
{


    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public  class BuildingComplete_OnSpawn_Patch
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
                try
                {
                    DebugViewClassPath.markCellToSpace(pos);
                    DebugViewClassPath.dig40Grid(pos);
                    Notification notification = new Notification("Need Rstart Game[需要重启游戏]", NotificationType.Bad,
                        (List<Notification> n, object d) => "over", null, true, 0f, null, null, null, false, false)
                    {
                        clearOnClick = true
                    };
                   
                    Notifier notifier = World.Instance.gameObject.AddComponent<Notifier>();
                    if (notifier != null)
                    {
                        notifier.Add(notification);
                    }

                }
                catch (Exception ex)
                {
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    Console.WriteLine("发生错误行号为:" + line);
                    Console.WriteLine(ex.ToString());
                }
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
            Console.WriteLine("OnActiveWorldChanged:切换了星球视角:");
            //markAllCellToSpace();
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
        /**
         * 
         * 不切割直接标记为太空背景.
         */
        public static void markCellToSpaceAnyway(Vector2 pos)
        {
            WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;

            for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
            {
                if (clusterDetailSave.overworldCells[i].poly.Contains(pos))
                {
                    clusterDetailSave.overworldCells[i].zoneType = SubWorld.ZoneType.Space;
                }
            }
        }
        public static void markCellToSpace(Vector3 pos)
        {
            WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
            //World.Instance.wo;
            int currentCell = Grid.PosToCell(pos);

            int markId = 0;
            WorldDetailSave.OverworldCell fmarkCellBlock = null;
            bool markFindOne = false;

            for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
            {
                if (clusterDetailSave.overworldCells[i].poly.Contains(pos))
                {
                    fmarkCellBlock = clusterDetailSave.overworldCells[i];
                    markId = i;
                    break;
                }
            }
            if (fmarkCellBlock == null)
            {
                Console.WriteLine("markCellToSpace没有找到背景单元格");
                return;
            }
            

            if (fmarkCellBlock.zoneType == SubWorld.ZoneType.Space)
            {
                Console.WriteLine("markCellToSpace已经是太空背景了");
                return;//如果已经是太空背景就返回了.不做
            }
            if (fmarkCellBlock.poly.Vertices.Count == 3)
            {
                Console.WriteLine("markCellToSpace已经切分为三角形了,直接标记为太空");
                markCellToSpaceAnyway(pos);

                return;//如果已经是三角形了,也不处理了.
            }
            if (fmarkCellBlock != null)
            {
                var list = splitOverworldCell(fmarkCellBlock);
                Console.WriteLine("切分多边形数量:" + list.Count);

                //list.Last().zoneType = SubWorld.ZoneType.Space;//设置最后一个区域为太空背景.
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].poly.Contains(pos))
                    {
                        list[i].zoneType = SubWorld.ZoneType.Space;
                        markFindOne = true;
                      //  digBlockByPos(list[i]);
                    }
                }//以下是方案二:
                for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
                {
                    WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];

                    if (isInBlock(overworldCell, pos))
                    {
                        markFindOne = true;
                    }
                }

                Console.WriteLine("在新的多边形里找到坐标:" + markFindOne);
                if (markFindOne == false)
                {
                    list.First().zoneType = SubWorld.ZoneType.Space;//没有找到的话,直接把第1个标记为真空
                }
                //删除原多边形
                Console.WriteLine("原区块数量:" + clusterDetailSave.overworldCells.Count);
                var succ = clusterDetailSave.overworldCells.Remove(fmarkCellBlock);
                Console.WriteLine("删除原多边形:" + succ);
                //添加新的多边形.
                list.ForEach(item => clusterDetailSave.overworldCells.Add(item));
                Console.WriteLine("新区块数量:" + clusterDetailSave.overworldCells.Count);
                // YellowAlertManager.Instance.
                // Game.Instance.Trigger();
                //


                /*


                 */
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
        public static bool isInBlock(WorldDetailSave.OverworldCell block, Vector2 pos)
        {

            Vector2 zero = Vector2.zero;
            int currentCell = Grid.PosToCell(pos);


            Polygon poly = block.poly;
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
                            if (num == currentCell)
                            {
                                block.zoneType = SubWorld.ZoneType.Space;
                                return true;
                            }

                        }
                    }
                    zero.x += 1f;
                }
                zero.y += 1f;
            }
            return false;

        }
        public static void dig40Grid(Vector2 pos)
        {
            int cell = Grid.PosToCell(pos);
            int digSize = 8;
            System.Random rnd = new System.Random();
            
     
            for (int i = -digSize; i < digSize ; i++)
            {
                for(int j = -digSize; j < digSize ; j++)
                {
                    int pcell = cell + Grid.WidthInCells*i+j;
                    pcell += rnd.Next(-2, 2);
                    if (pcell>0)
                        SimMessages.Dig(pcell, -1, false);//.
                }
            }

        }
        public static void digBlockByPos(WorldDetailSave.OverworldCell block)
        {

            string msg = "";
            block.poly.Vertices.ForEach(v =>
            {
                msg += Grid.PosToCell(v) + ",";
                SimMessages.Dig(Grid.PosToCell(v), -1, false);//.
            });
            Console.WriteLine("挖掉所有物质:" + msg);
        }
        /**
         * 按中线生成多边形. 6边形切成7份. 5边形切成6份.
         * 如,六边形7等分,
         *  TODO
         */
        public static Polygon[] splitPolygon5(Polygon src)
        {
            //多边形切割成多份.
            Polygon[] result = new Polygon[4];
            /*src.Vertices;
            PolygonUtils;*/
            return result;
        }
        /**
         * 多边形切割成多份.
        * 按中点切成三角形.6边形切成6份.
        *  
        * 
        */
        public static Polygon[] splitPolygon3(Polygon src)
        {

            if (src.Vertices.Count == 3)
            {
                return new Polygon[1] { src };
                //三角形不能再切了
            }
            Polygon[] result = new Polygon[src.Vertices.Count];
            var centerPoint = PolyUT.GetCenterOfGravityPoint(src.Vertices);
            for (int i = 0; i < src.Vertices.Count; i++)
            {

                //  List<Vector2> listnew = new List<Vector2>();
                Polygon polynew = new Polygon();
                Vector2 aPoint = src.Vertices[i];
                Vector2 bPoint;
                if (i + 1 == src.Vertices.Count)
                {
                    bPoint = src.Vertices[0];
                }
                else
                {
                    bPoint = src.Vertices[i + 1];
                }

                polynew.Add(centerPoint);//生成三形.
                polynew.Add(aPoint);
                polynew.Add(bPoint);

                result[i] = polynew;
                ;
            }

            return result;
        }
        /**
         * 取中心点切割成一个十字块.
         * 剩下的点切为多边形.采用挖空中间的形状
         * 
         */
        public static Polygon[] splitPolygon4(Polygon src)
        {
            int blockSize = 20;//中间的洞大小
            if (src.Vertices.Count == 3)
            {
                return new Polygon[1] { src };
                //三角形不能再切了
            }
            Polygon[] result = new Polygon[src.Vertices.Count];
            var centerPoint = PolyUT.GetCenterOfGravityPoint(src.Vertices);

            var blockCenter = new Polygon();
            //  blockCenter.Clip 
            blockCenter.Add(new Vector2(centerPoint.x + blockSize, centerPoint.y + blockSize));
            blockCenter.Add(new Vector2(centerPoint.x + blockSize, centerPoint.y - blockSize));
            blockCenter.Add(new Vector2(centerPoint.x - blockSize, centerPoint.y - blockSize));
            blockCenter.Add(new Vector2(centerPoint.x - blockSize, centerPoint.y + blockSize));


            for (int i = 0; i < src.Vertices.Count; i++)
            {

                //  List<Vector2> listnew = new List<Vector2>();
                Polygon polynew = new Polygon();
                Vector2 aPoint = src.Vertices[i];
                Vector2 bPoint;
                if (i + 1 == src.Vertices.Count)
                {
                    bPoint = src.Vertices[0];
                }
                else
                {
                    bPoint = src.Vertices[i + 1];
                }

                polynew.Add(centerPoint);//生成三形.
                polynew.Add(aPoint);
                polynew.Add(bPoint);

                result[i] = polynew;
                ;
            }

            return result;
        }

        /**
         * 把多边形Cell转化为多个多边形.
         * 
         */
        public static List<WorldDetailSave.OverworldCell> splitOverworldCell(WorldDetailSave.OverworldCell oriCell)
        {
            var outs = new List<WorldDetailSave.OverworldCell>();
            Polygon[] outPolys = splitPolygon3(oriCell.poly);
            for (int i = 0; i < outPolys.Length; i++)
            {
                var outPoly = outPolys[i];//三角形是等分的

                WorldDetailSave.OverworldCell cell = new WorldDetailSave.OverworldCell();
                cell.poly = outPoly;
                cell.tags = oriCell.tags;
                cell.zoneType = oriCell.zoneType;
                outs.Add(cell);
            }
            return outs;
        }







        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch("LoadGeneratedBuildings")]
        public class GeneratedBuildingsPatch
        {
            private static void Prefix()
            {
                Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.VACUUMSPACEMOD.DESC",
                    "make bomb destory all 10x10 block. background will delete. need restart game." +
                    "炸弹会删除背景,范围10x10 .需要重新加载才能看到效果,删除的背景是三角形" });
                Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.VACUUMSPACEMOD.EFFECT",
                    "Bomb destory 10x10 ,backgroun will deleted\n" +
                    "范围10x10,炸弹会删除背景," });
                Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.VACUUMSPACEMOD.NAME", "SpaceBomb" });

                ModUtil.AddBuildingToPlanScreen("Base", "VacuumSpaceMod");
            }
        }

    }

}
