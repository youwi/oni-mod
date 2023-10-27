using HarmonyLib;
using Klei;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteAsteroid
{
  
    public class DeleteOnSave
    {

        [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save))]
        public static void Postfix(MutantPlant __instance)
        {
            //SaveLoader.Instance.Save(filename, false, true);
            DeleteMarkedAsteroid();
        }
        /*
         * 
         * 删除星球mod
         * 当殖民地改名为 "delete" 时
         * 保存再重新加载就可以 删除 殖民地.
         * 需要改名的支持.
         */

        public static void DeleteMarkedAsteroid()
        {
            //  WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
            // SaveGame.Instance.gameObject.as
            //SaveLoader.Instance.GetMyWorldDetail(clusterDetailSave);
            // SaveLoader.Instance.gameObject.get
            //SaveLoader.Instance.GetComponent<AsteroidGridEntity>();
            //AsteroidConfig ac =SaveLoader.Instance.GetComponent<AsteroidConfig>();

            //SaveLoader.Instance.getW;
            //ClusterManager.Instance.GetWorld(world_id);
            //ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();

            // var world = ClusterManager.Instance.GetWorld(row.Key);


            //ClusterManager.Instance.WorldContainers. Clear();//删除所有
            // ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted;
            //SaveLoader.Instance.
            //ClusterGrid.Instance.;
            // ClusterGridEntity.in
            //if (m_worldContainers[i].IsDiscovered && !m_worldContainers[i].IsModuleInterior)
            // AsteroidGridEntity.ins
            //ClusterManager.Instance.;
            //AsteroidConfig s;
            //s.get
            //var world in ClusterManager.Instance.WorldContainers)
            //int max = ClusterManager.Instance.WorldContainers.Count;
            var worlds = ClusterManager.Instance.WorldContainers;
            //WorldContainer markDeleteWorld = null;
            for (int i = 0; i < worlds.Count; i++)
            {
                if (worlds.Count > 1)
                {
                    var world = worlds[i];
                    if(world == null)
                    {
                        continue;
                    }
                    if (world.IsStartWorld || world.IsModuleInterior)
                    {
                        //初始星不能删除. 火箭不能删除
                        continue;
                    };
                    var gridEntity = world.GetComponent<ClusterGridEntity>();
                    if (gridEntity.Name == "delete")  // 获取右上角底层的实体名.
                    {
                        worlds.Remove(world);// markDeleteWorld = world;
                    };
                    //循环删除
                }
            }
        }

    }
}
