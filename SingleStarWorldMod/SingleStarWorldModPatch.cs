using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

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
          int max = placement.allowedRings.max+1;

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
      global::Debug.LogWarning("强制更新星图");
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
        || clName == "expansion1::clusters/OneStarDLC";
       global::Debug.LogWarning("IsMyOneWorld: " + clName + " ->" + outFlag);
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
    }
  }

    [HarmonyPatch(typeof(WorldGen), nameof(WorldGen.SetWorldSize))]
    public static class WorldGen_SetWorldSize_Patch
    {


        public static void Postfix(WorldGen __instance,int width, int height)
        {
            if (SingleStarWorldModPatch.IsMyOneWorld())
            {
               
                 global::Debug.LogWarning($"reset size:{width} ,{height}, GridSize: { Grid.HeightInCells} ,{Grid.WidthInCells}");
                if(width>100)
                  __instance.data.world = new Chunk(0, 0, width - 100, height);
               // Grid.HeightInCells = height;
                Grid.WidthInCells -= 100;
             
                   
            }
                
        }
    }




}
