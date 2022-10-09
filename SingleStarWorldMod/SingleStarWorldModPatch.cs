using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ProcGen;
using ProcGenGame;
using UnityEngine;

namespace SingleStarWorldMod
{
    [HarmonyPatch(typeof(Cluster), nameof(Cluster.AssignClusterLocations))]
    public class SingleStarWorldModPatch
    {
        public static void Postfix(Cluster __instance)
        {
            //强制让星云挤在一起.
            //new  Cluster().AssignClusterLocations;
            if (__instance == null) return;
            if (__instance.poiPlacements.Count > 20) return;
            SeededRandom rnd = new SeededRandom(2123121);

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
                    int max = placement.allowedRings.max;

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
  
    }

  }
