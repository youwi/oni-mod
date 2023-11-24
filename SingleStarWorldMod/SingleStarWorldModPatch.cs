using HarmonyLib;
using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;

namespace SingleStarWorldMod
{

    // Add minibase asteroid cluster
    [HarmonyPatch(typeof(Db), "Initialize")]
    public class Db_Initialize_Patch : KMod.UserMod2
    {
        public static string ModPath;
        public static string ClusterName = "OneStar";
        public static string ClusterDescription = " start game only one star.";
        public static string ClusterIconName = "OneStarCluster";
        public override void OnLoad(Harmony harmony)
        {

            ModPath = mod.ContentPath;
            base.OnLoad(harmony);

        }
        public static void Prefix()
        {

            Strings.Add($"STRINGS.WORLDS.{ClusterName.ToUpperInvariant()}.NAME", ClusterName);
            Strings.Add($"STRINGS.WORLDS.{ClusterName.ToUpperInvariant()}.DESCRIPTION", ClusterDescription);
            Strings.Add($"STRINGS.CLUSTER_NAMES.{ClusterIconName.ToUpperInvariant()}.NAME", ClusterIconName);
            Strings.Add($"STRINGS.CLUSTER_NAMES.{ClusterIconName.ToUpperInvariant()}.DESCRIPTION", ClusterDescription);
            //以下可能出错
            //string spritePath = System.IO.Path.Combine(ModPath, ClusterIconName) + ".png";
            //Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            //ImageConversion.LoadImage(texture, File.ReadAllBytes(spritePath));
            //Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 400f, 400f), Vector2.zero);
            //Assets.Sprites.Add(ClusterIconName, sprite);
        }
    }


    [HarmonyPatch(typeof(Cluster), nameof(Cluster.AssignClusterLocations))]
    public static class SingleStarWorldModPatch
    {
        public static void Postfix(Cluster __instance)
        {
            //强制让星云挤在一起.
            //new  Cluster().AssignClusterLocations;
            if (__instance == null) return;
            if (!IsMyOneWorld())
            {
                return;//如果不是我的这个星
            }


            SeededRandom rnd = new SeededRandom(new System.Random().Next(1, 10000));

            ClusterLayout clusterLayout = SettingsCache.clusterLayouts.clusterCache[__instance.Id];
            List<SpaceMapPOIPlacement> allPlacements = (clusterLayout.poiPlacements == null) ? new List<SpaceMapPOIPlacement>() : new List<SpaceMapPOIPlacement>(clusterLayout.poiPlacements);

            int count = 0;
            allPlacements.ForEach(x => count += x.pois.Count);

            List<AxialI> locations = new List<AxialI>();

            List<AxialI> existLocations = new List<AxialI>();



            for (int i = 0; i < allPlacements.Count; i++)
            {
                SpaceMapPOIPlacement placement = allPlacements[i];

                List<string> pois = new List<string>(placement.pois);
                List<AxialI> allPoint = AxialUtil.GetAllPointsWithinRadius(AxialI.ZERO, placement.allowedRings.max);

                for (int j = 0; j < placement.numToSpawn && j < pois.Count; j++)
                {
                    int min = placement.allowedRings.min;
                    int max = placement.allowedRings.max + 1;

                    AxialI location = AxialI.ZERO;
                    int loopCount = 0;

                    do
                    {  // 手动生成位置,或直接取位置
                       // int a = rnd.RandomRange(0, max);
                       // int b = rnd.RandomRange(0, max);
                       // location = new AxialI(a * rand1(), b * rand1());
                        location = allPoint[rnd.RandomRange(0, allPoint.Count)];


                        loopCount++;
                        if (loopCount > 1000) break;

                    } while (location == AxialI.ZERO
                    || existLocations.Contains(location)
                    || AxialUtil.GetDistance(AxialI.ZERO, location) < min

                    );

                    existLocations.Add(location);
                    __instance.poiPlacements[location] = pois[j];

                }
            }
            global::Debug.Log("强制更新星图");
        }

        static int randSeed = 0;
        public static int rand1()
        {
            //随机生成正负1
            System.Random rand = new System.Random(randSeed);

            randSeed++;
            bool flag = rand.Next(1, 10) <= 5f;
            if (flag)
            {
                return 1;
            }
            else
                return -1;
        }
        public static void randWithOut(List<AxialI> ori, AxialI obj)
        {
            for (int i = 0; i < 20; i++)
            {

            }
        }
        public static bool IsMyOneWorld()
        {
            string clName = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
            bool outFlag =
              clName == "clusters/OneStar"
              || clName == "clusters/OneStarCluster"
              || clName == "expansion1::clusters/OneStar"
              || clName == "expansion1::clusters/OneStarCluster"
              || clName == "expansion1::clusters/OneStarDLC"
            ;
            global::Debug.Log("IsMyOneWorld: " + clName + " ->" + outFlag);

            return outFlag;
        }
        public static bool IsMiniBase()
        {
            string clName = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;

            // expansion1::clusters/MiniBaseDLC

            bool outFlag = clName.Contains("MiniBase");

            return outFlag;
        }
    }

    // Bypass and rewrite world generation
    [HarmonyPatch(typeof(WorldGen), nameof(WorldGen.RenderOffline))]
    public static class WorldGen_RenderOffline_Patch
    {


        public static void Postfix(WorldGen __instance, ref bool __result, ref Sim.Cell[] cells, ref Sim.DiseaseCell[] dc, int baseId)
        {
            if (!SingleStarWorldModPatch.IsMyOneWorld())
                return;
            __result = GenWorldFix.WorldReplaceFix(__instance, ref cells, ref dc, baseId);
            global::Debug.Log($"RenderOffline:  GridSize: {Grid.HeightInCells} ,{Grid.WidthInCells}");
        }
    }
    [HarmonyPatch(typeof(Cluster), "Load")]
    public static class Cluster_Load_Patch
    {
        //public static void Prefix(Cluster __instance)
        //{
        //    try
        //    {
        //        //  YamlIO.Save(SettingsCache.clusterLayouts, "D:/clusterLayouts.yaml"); ;
        //        //  WorldGen.LoadSettings();
        //        //  __instance.Save(WorldGen.WORLDGEN_SAVE_FILENAME + "TTTest");
        //        // FastReader fastReader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
        //        // __instance.saveAsText
        //    }
        //    catch (Exception e)
        //    {
        //        global::Debug.Log(">>>>>" + e.Message);
        //    }

        //    global::Debug.Log($"-----Cluster.Load Prefix Grid:{Grid.HeightInCells} ,{Grid.WidthInCells}");
        //    //  global::Debug.Log($"--LoadFromWorldGen size----->-- {__instance.}");

        //}
        public static void Postfix(Cluster __instance, ref Cluster __result)
        {
            //生成之后查看世界大小
            //__instance.ClusterLayout.
            global::Debug.Log($"-----Cluster.load Postfix Cluster.size----->-- {__result.size}");
            if (SingleStarWorldModPatch.IsMyOneWorld() || SingleStarWorldModPatch.IsMiniBase())
            {
                __result.size.x -= 99;
                global::Debug.Log($"-----修改的地图大小:----->-- {__result.size}");

            }


            //这个值可以改变大小
            global::Debug.Log($"-----Cluster.Load Postfix Grid.size:{Grid.HeightInCells} ,{Grid.WidthInCells}");
        }
        //this.m_clusterLayout
    }

    //[HarmonyPatch(typeof(WorldGen), nameof(WorldGen.SetWorldSize))]
    //public static class WorldGen_SetWorldSize_Patch
    //{


    //    public static void Postfix(WorldGen __instance,int width, int height)
    //    {
    //        if (SingleStarWorldModPatch.IsMyOneWorld())
    //        {

    //            global::Debug.Log($"--->reset size:{width} ,{height}, GridSize: { Grid.HeightInCells} ,{Grid.WidthInCells}");
    //            global::Debug.Log($"--->WorldGen.GetSize:{__instance.GetSize()}");

    //            //此时没有初始化,数字为0
    //            if (width>100)
    //              __instance.data.world = new Chunk(0, 0, width - 100, height);
    //           // Grid.HeightInCells = height;
    //           // Grid.WidthInCells -= 100;

    //            //修改世界布局.多个图拼在一起.
    //           //var lv= new LevelLayerSettings();
    //            //lv.LevelLayers.
    //            //WorldLayout.ma
    //            //重置大小?
    //           // Grid.InitializeCells();
    //           // WorldLayout.SetLayerGradient(SettingsCache.layers.LevelLayers);

    //        }

    //    }
    //}

    //[HarmonyPatch(typeof(Grid), nameof(Grid.InitializeCells))]
    //public static class Grid_InitializeCells_Path
    //{
    //    public static void Postfix()
    //    {
    //       // GridSettings.Reset(this.m_clusterLayout.size.x, this.m_clusterLayout.size.y);

    //        global::Debug.Log($"InitializeCells Grid:{Grid.HeightInCells} ,{Grid.WidthInCells}");
    //        if (Grid.WidthInCells > 100)
    //        {
    //           // GridSettings.Reset(200, 200);//强制设置为200x200,会崩溃
    //           // Grid.WidthInCells = 100;
    //            Debug.Log(new System.Diagnostics.StackTrace().ToString());
    //        }
    //        //worldGen.GetSize();
    //       // Cluster.Load();
    //        //SaveLoader.OnSpawn();
    //    }
    //}

    //[HarmonyPatch(typeof(GridSettings), nameof( GridSettings.Reset))]
    //public static class GridSettings_Reset_Path
    //{
    //    public static void Prefix(int width, int height)
    //    {
    //       // ListPool<SimSaveFileStructure, SaveLoader>.PooledList pooledList = ListPool<SimSaveFileStructure, SaveLoader>.Allocate();
    //       // this.m_clusterLayout.LoadClusterLayoutSim(pooledList);
    //        global::Debug.Log($"----GridSettings.Reset :{width} ,{height}");
    //    }
    //}

    //GridSettings.Reset
    /* 
     [HarmonyPatch(typeof(Cluster), "BeginGeneration")]
     public static class Cluster_BeginGeneration_Patch
     {
         public static void Postfix(Cluster __instance) 
         {
             //生成之后查看世界大小


             List<WorldGen> list = new List<WorldGen>(__instance.worlds);
             foreach(var wg in list )
             {
                 global::Debug.Log($"----BeginGeneration size----->-- {wg.GetSize()}");
             }

             global::Debug.Log($"----Cluster_BeginGeneration_Patch:{Grid.HeightInCells} ,{Grid.WidthInCells}");
         }
         //this.m_clusterLayout
     }
     [HarmonyPatch(typeof(SaveLoader), "LoadFromWorldGen")]
     public static class SaveLoader_LoadFromWorldGen_Patch
     {
         public static void Prefix(SaveLoader __instance)
         {
             var tt = __instance.ClusterLayout;
             if(tt== null)
             {
                 global::Debug.Log($"-----LoadFromWorldGen Prefix:----->- null");
             }
             else
             {
                 global::Debug.Log($"-----LoadFromWorldGen Prefix:----->- {tt.size}");
             }

             global::Debug.Log($"-----LoadFromWorldGen Prefix:Grid:{Grid.HeightInCells} ,{Grid.WidthInCells}");
             //  global::Debug.Log($"--LoadFromWorldGen size----->-- {__instance.}");
             // Cluster.Save();

         }
         public static void Postfix(SaveLoader __instance)
         {
             //生成之后查看世界大小
             //__instance.ClusterLayout.
             global::Debug.Log($"-----LoadFromWorldGen Postfix:----->-- {__instance.ClusterLayout.size}");

             global::Debug.Log($"-----LoadFromWorldGen Postfix Grid:{Grid.HeightInCells} ,{Grid.WidthInCells}");
         }
         //this.m_clusterLayout
     }
     //WORLDGEN COMPLETE

     // 
    */




}
